### 使用HTTP推送设备数据

上报属性例子: 

POST /{productId}/{deviceId}/properties/report
Authorization:{产品或者设备中配置的Token}
Content-Type: application/json

{
 "properties":{
   "temp":38.5
 }
}

上报事件例子:

POST /{productId}/{deviceId}/event/{eventId}
Authorization:{产品或者设备中配置的Token}
Content-Type: application/json

{
 "data":{
   "address": ""
 }
}
