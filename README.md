# SqlApi

與MS Sql資料庫交易

## RESTful ReadDataController

get sql查詢語法

參數 strcommand

Response body
```
[
  {
    "TA001": "0001",
    "TA002": "小明",
    "TA003": 1,
    "TA004": 3.140
  },
  {
    "TA001": "0002",
    "TA002": "曉華",
    "TA003": 1,
    "TA004": 5.600
  }
]
```

## 非RESTful SqlController

1. /api/sql/readData

執行查詢語法

2. /api/sql/writeData

執行INSERT、UPDATE、DELTET與其他語法