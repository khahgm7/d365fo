pageViews
| summarize count(), sum(duration), arg_max(timestamp, *) by user_Id, name
| project 
    user_Id
    , name
    , AccessTimes = count_
    , LatestAccess = timestamp
    , TotalDurationInSeconds = round(sum_duration/1000, 2)
| order by AccessTimes desc