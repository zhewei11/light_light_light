#include "communication.h"

extern TaskHandle_t LED_UPDATE;
extern TaskHandle_t WIFI_HANDLE;

HTTPClient http;

Communication::Communication(){
    
}

void Communication::init(){
    connect();
    OTA();
}

void Communication::OTA()
{
    ArduinoOTA.onStart([]() {
        vTaskDelete(WIFI_HANDLE);
        vTaskDelete(LED_UPDATE);
        String type;
        if (ArduinoOTA.getCommand() == U_FLASH)
        {
            type = "sketch";
        }
        else
        { // U_FS
            type = "filesystem";
        }

        // NOTE: if updating FS this would be the place to unmount FS using FS.end()
        Serial.println("Start updating " + type);
    });
    ArduinoOTA.onEnd([]() {
        Serial.println("\nEnd");
    });
    ArduinoOTA.onProgress([](unsigned int progress, unsigned int total) {
        Serial.printf("Progress: %u%%\r", (progress / (total / 100)));
    });
    ArduinoOTA.onError([](ota_error_t error) {
        Serial.printf("Error[%u]: ", error);
        if (error == OTA_AUTH_ERROR)
        {
            Serial.println("Auth Failed");
        }
        else if (error == OTA_BEGIN_ERROR)
        {
            Serial.println("Begin Failed");
        }
        else if (error == OTA_CONNECT_ERROR)
        {
            Serial.println("Connect Failed");
        }
        else if (error == OTA_RECEIVE_ERROR)
        {
            Serial.println("Receive Failed");
        }
        else if (error == OTA_END_ERROR)
        {
            Serial.println("End Failed");
        }
    });
    ArduinoOTA.begin();
}

void Communication::connect(){
    int n = WiFi.scanNetworks();
    for (int i = 0; i < n; ++i){
        String ssid = WiFi.SSID(i);
        if (ssid == WIFI_SSID1 || ssid == WIFI_SSID2 || ssid == WIFI_SSID3){
            String pass = (ssid == WIFI_SSID1) ? WIFI_PASS1 : (ssid == WIFI_SSID2) ? WIFI_PASS2 : WIFI_PASS3;
            WiFi.begin(ssid.c_str(), pass.c_str());
            Serial.println();
            Serial.print("Connecting to ");
            Serial.println(ssid);
            break;
        }
    }

    Serial.print("Waiting for connection");
    int connection_times = 0;
    while (WiFi.status() != WL_CONNECTED){
        vTaskDelay(pdMS_TO_TICKS(500));
        Serial.print(".");
        connection_times++;
        if (connection_times >= WIFI_CONNECT_RETRY){
            Serial.println();
            Serial.println("Connection Failed! Rebooting...");
            vTaskDelay(pdMS_TO_TICKS(1000));
            ESP.restart();
        }
    }

    Serial.println("");
    Serial.print("IP address: ");
    Serial.println(WiFi.localIP());
    Serial.println(WiFi.gatewayIP());
}

void Communication::feed_color_param(ValueParam* p, String s){
    int comma = s.indexOf(',');
    uint32_t upper = s.substring(0, comma).toInt();
    p->func = (SchedulerFunc)((upper >> 16)    & 0xff);
    p->range = (upper >> 8) & 0xff;
    p->lower = upper        & 0xff;
    
    uint32_t lower = s.substring(comma+1).toInt();
    p->p1 = (lower >> 8) & 0xff;
    p->p2 = lower        & 0xff;
}

int Communication::feed_data(Mode* m, String s){
    int start=0, len=0;
    uint32_t checksum = 0;
    if(s != "ERROR!!")
      Serial.println(s);
    for (int i=0; i<s.length(); i++){
        // If the first charactor is not `M`, return as error
        if (i==0 && s[0] != 'M') return 1;
        if ((s[i] >= '0' && s[i] <= '9') || s[i] == ',') continue;

        // If there is multi times non-number charactor appearing, it
        // will skip them till next number comes
        if (start != i){
            // Length of string must larger than 0
            String meta = s.substring(start, i);

            /*
            // If it failed to convert, it will return 0.
            if (len == 0)       // Mode Type
                (*m).mode = (MODES)meta.toInt();
            else if (len == 1)  // Start time
                (*m).start_time = meta.toInt();
            else{               // Other parameter
                (*m).param[len-2] = meta.toInt();
                checksum += (*m).param[len-2];
            }
            len++;
            */
            char key = s[start-1];
            switch(key){
                case 'M': (*m).mode = (MODES)meta.toInt(); break;
                case 'S': (*m).start_time = meta.toInt();  break;
                case 'D': (*m).duration = meta.toInt();    break;
                case 'X': feed_color_param(&(m->XH), meta);break;
                case 'Y': feed_color_param(&(m->XS), meta);break;
                case 'Z': feed_color_param(&(m->XV), meta);break;
                case 'U': feed_color_param(&(m->YH), meta);break;
                case 'V': feed_color_param(&(m->YS), meta);break;
                case 'W': feed_color_param(&(m->YV), meta);break;
                case 'P': 
                    int comma = meta.indexOf(',');
                    uint32_t upper = meta.substring(0, comma).toInt();
                    m->param[0] = (upper >> 8) & 0xff;
                    m->param[1] = upper        & 0xff;
                    
                    uint32_t lower = meta.substring(comma+1).toInt();
                    m->param[2] = (lower >> 8)  & 0xff;
                    m->param[3] = lower         & 0xff;
                    break;
            }
        }
        
        if (s[i] == ';'){
            /*
            // Perform checksum and break
            String meta = s.substring(start, i);
            if (checksum & 0xff == meta.toInt())
                return len-2;
            else    // Checksum failed
            */
            return 0;
        }
        start = i+1;
    }
    // End without checksum, unusual request
    return 0;
}

bool Communication::receive(Mode* m, int current_id){
    if (WiFi.status() == WL_CONNECTED){
        /* Request data from server */
        String url = "http://" + WiFi.gatewayIP().toString()  + String(WIFI_REQUEST_URL) + "?id=" + current_id + "&luxid=" + LUX_ID;
        //Serial.println(url);
        http.begin(url);
        int httpCode = http.GET();
        
        if (httpCode == 200) {  // Check if the request was successful
            String web_data = http.getString();

            //Serial.print("\n\nnumber: ");
            //Serial.println(current_id);
            if (feed_data(m, web_data) != 0){
                // Message error, Report it
                http.end();
                return false;
            }
            #ifdef DEBUGGER_TASK_REPORT
            PrintMode(m);
            #endif
            PrintMode(m);
        } else {
            // Handle errors or unexpected response codes
            http.end();
            return false;  // Return false if the request failed
        }
        http.end();
    }
    else {
        WifiErrorHandle();
        return false;  // Return false if not connected to WiFi
    }
    return true;
}


time_t Communication::check_start_time(uint8_t id, MODES mode, uint8_t* force_start){
    if (WiFi.status() == WL_CONNECTED){
        /* Request data from server */
        String url ="http://" + WiFi.gatewayIP().toString() +  String(WIFI_TIME_CHECK_URL) + "?id=" + id + "&effect=" + mode;
        // String url ="http://192.168.0.250" +  String(WIFI_TIME_CHECK_URL) + "?id=" + id + "&effect=" + mode;
        //Serial.println(url);
        http.begin(url);
        int httpCode = http.GET();
        if (httpCode == 200) {  // Successful request
            String web_data = http.getString();
            if (web_data.length() > 0) {
                if (web_data[0] == 'C')
                    *force_start = 2;
                else if (web_data[0] == 'M')
                    *force_start = 1;
                else
                    *force_start = 0; 
                return web_data.substring(1).toInt();
            }
        }
        http.end();
    }
    else WifiErrorHandle();
    return 0 - 1;
}


void Communication::WifiErrorHandle() {
    #ifdef DEBUGGER
    Serial.println("Connection Failed! Retrying...");
    #endif
    
    // 重试连接WiFi
    int retryCount = 0;
    while (WiFi.status() != WL_CONNECTED && retryCount < WIFI_CONNECT_RETRY) {
        WiFi.disconnect();
        vTaskDelay(pdMS_TO_TICKS(1000)); // 等待1秒再重试
        WiFi.begin(WIFI_SSID1, WIFI_PASS1);
        Serial.print("Retrying connection");
        while (WiFi.status() != WL_CONNECTED && retryCount < WIFI_CONNECT_RETRY) {
            vTaskDelay(pdMS_TO_TICKS(500)); // 每0.5秒检查一次连接状态
            Serial.print(".");
            retryCount++;
        }
    }
    
    if (WiFi.status() == WL_CONNECTED) {
        Serial.println();
        Serial.println("Reconnected successfully!");
    } else {
        Serial.println();
        Serial.println("Reconnection Failed! Please check your network settings.");
    }
}


void Communication::updateOTA(){
    ArduinoOTA.handle();
}

void PrintColorSch(ValueParam* v){
    Serial.print("  func:");
    Serial.print(v->func);
    Serial.print(", range:");
    Serial.print(v->range);
    Serial.print(", lower:");
    Serial.print(v->lower);
    Serial.print(", p1:");
    Serial.print(v->p1);
    Serial.print(", p2:");
    Serial.println(v->p2);
}

void PrintMode(Mode* m){
    Serial.print("Mode:");
    Serial.print(m->mode);
    Serial.print(", start:");
    Serial.print(m->start_time);
    Serial.print(", dur:");
    Serial.println(m->duration);

    Serial.print("XH: ");
    PrintColorSch(&(m->XH));
    Serial.print("XS: ");
    PrintColorSch(&(m->XS));
    Serial.print("XV: ");
    PrintColorSch(&(m->XV));

    Serial.print("YH: ");
    PrintColorSch(&(m->YH));
    Serial.print("YS: ");
    PrintColorSch(&(m->YS));
    Serial.print("YV: ");
    PrintColorSch(&(m->YV));

    Serial.println("Param:");
    for (int i=0; i<META_PARAMETER_BUF_SIZE; i++){
        Serial.print(m->param[i]);
        Serial.print(", ");
    }
    Serial.println("");
}