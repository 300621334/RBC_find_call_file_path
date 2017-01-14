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