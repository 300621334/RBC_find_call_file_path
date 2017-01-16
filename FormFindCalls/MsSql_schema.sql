--https://www.techonthenet.com/sql_server/index.php
--select * from BB_PRODUCT where type=type or idproduct=1;--type=type meand if type matches ANY value under type then that's selected. i.e. Everything

--sp_help rbc_contacts; -- same as desc  rbc_contacts; in Oracle.

create table rbc_contacts
(
contact_id varchar(20),
audio_module_no  varchar(12),
audio_ch_no  varchar(12),
pcd1_ID  varchar(15),
pcd1_value  varchar(15),
pcd2_ID  varchar(15),
pcd2_value  varchar(15)
);

insert into 
rbc_contacts(contact_id,audio_module_no,audio_ch_no,pcd1_ID,pcd1_value) 
values('9128328744020010000','471011','14029946','RG','insurence');
insert into 
rbc_contacts(contact_id,audio_module_no,audio_ch_no,pcd2_ID,pcd2_value) 
values('9128328744020010001','471012','14029943','call purpose','credit');
insert into 
rbc_contacts(contact_id,audio_module_no,audio_ch_no,pcd2_ID,pcd2_value) 
values('9128328744020010003','471010','14029947','call purpose','debit');
insert into 
rbc_contacts(contact_id,audio_module_no,audio_ch_no,pcd2_ID,pcd2_value) 
values('9128328744020010004','471015','329947','call purpose','loan');


alter table rbc_contacts add 
start_time datetime,
end_time datetime;

--http://stackoverflow.com/questions/12957635/sql-query-to-insert-datetime-in-sql-server
--implicit casting of varchar to datetime etc: https://msdn.microsoft.com/en-us/library/ms187928.aspx
UPDATE rbc_contacts set start_time='20170101 10:34:09 AM', end_time='20170101 10:36:09 AM' where contact_id = '9128328744020010000';
UPDATE rbc_contacts set start_time='20170110 11:59:09 AM', end_time='20170110 12:04:09 PM' where contact_id = '9128328744020010001';
UPDATE rbc_contacts set start_time='20170115 11:58:09 PM', end_time='20170116 01:02:09 AM' where contact_id = '9128328744020010003';
UPDATE rbc_contacts set start_time='20170120 10:01:09 AM', end_time='20170120 10:07:09 AM' where contact_id = '9128328744020010004';

update rbc_contacts set pcd1_value = 'insurance' where contact_ID = 9128328744020010004;