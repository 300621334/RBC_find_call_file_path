declare @x datetime2;
select @x =  cast(substring(convert(varchar, start_time, 113),13,8) as time) from rbc_contacts where contact_ID=9128328744020010003;
print(@x);


--substring(start_time, 12, 8)