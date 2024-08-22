declare
  @CustAccount nvarchar(40) = '' --Look up for a specific customer account; otherwise, look up for all accounts if leave this parameter empty
, @DataAreaId nvarchar(4) = '' --A specific company or all company if leave this parameter empty
;

select t1.DATAAREAID as [Company]
, t1.ACCOUNTNUM as [Customer accont], t2.NAME as [Customer name]
, t4.DESCRIPTION as [Address name], t5.ADDRESS as [Address], t3.POSTALADDRESSROLES as [Address purpose]
, case t3.ISPRIMARY 
	when 1 then 'Yes'
	else 'No'
  end as [Is primary] 
from CUSTTABLE as t1
join DIRPARTYTABLE as t2 on t2.RECID = t1.PARTY
join DIRPARTYLOCATION as t3 on t3.PARTY = t2.RECID
join LOGISTICSLOCATION as t4 on t4.RECID = t3.LOCATION
join LOGISTICSPOSTALADDRESS as t5 on t5.LOCATION = t4.RECID
where (@CustAccount = '' or t1.ACCOUNTNUM = @CustAccount)
and (@DataAreaId = '' or t1.DATAAREAID = @DataAreaId)
order by t1.DATAAREAID asc, t1.ACCOUNTNUM asc