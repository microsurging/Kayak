### 认证说明

CONNECT报文:
```text
clientId: 设备ID
password: md5(timestamp+"|"+secureKey)
 ```

说明: key在创建设备产品或设备实例时进行配置.
    timestamp为当前时间戳(毫秒), 与服务器时间不能相差5分钟.
        md5为32位, 不区分大小写.