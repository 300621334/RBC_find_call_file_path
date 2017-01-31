--(localdb)\MSSQLLocalDB
-------------
sp_help 'employee'; --Works e or eout quotes, same as desc table in oracle;
sp_rename 'contacts.contacts_idx', 'contacts_index_cname', 'INDEX';--to rename an index
SELECT * FROM master.sys.sql_logins; --all logins. Pwd shown as hashed value. Older version use view "master.sys.syslogins"
SELECT * FROM master.sys.database_principals; --find USERs
--To create a USER, need to create a LOGIN 1st,

-------------

-------------
SELECT GETDATE(); --select sysdate from dual;
SELECT 'something'; --select 'something' from dual;
select * from dual;
select distinct contact_ID from xxx;
-------------
select * from xxx where UPPER(pcd1_value) LIKE ('insur%');--by default, MySQL does not consider the case of the strings. Not quite http://dba.stackexchange.com/questions/15250/how-to-do-a-case-sensitive-search-in-where-clause

select * from xxx where pcd1_value LIKE 'insur%';--match any # of chars
select * from xxx where pcd2_value LIKE '_ebit';--exact 1 char
select * from xxx where pcd1_value LIKE 'insur~%' ESCAPE '~';--escape %
select * from xxx where pcd1_value LIKE 'insur[ae]nce';--1 char exact. either 'a' or 'e'
select * from xxx where pcd1_value LIKE 'insur[^e]nce';--insur'e'nce excluded
-------------
select * from xxx where start_time BETWEEN cast('1/1/2017 10:34:09 AM' AS datetime2) AND convert(datetime, '1/15/2017 11:58:09 PM');
select * from xxx where EXISTS(select * from xxx where pcd1_value IS NOT NULL);
---
select audio_module_no + audio_ch_no AS "f i l e L o c a t i o n" from xxx ;--catenate strings
select audio_module_no +0+ audio_ch_no from xxx AS fileLocation;--str cast to int & added. AS in wrong place but query works!!!
select audio_module_no +''+ audio_ch_no AAA from xxx;--catenated
---
select contact_ID as aaa from xxx;
---
select * from aaa;
create table aaa(x int default 1,y int default 2,z int); --default for z is NULL
insert into aaa(x,y,z) values(default, 8, 9);
alter table aaa add p int identity(1,1);--even two older rows got auto-generated identity values :-)
insert into aaa default values;
UPDATE aaa set x=y from aaa where y=8;
rollback;
TRUNCATE table aaa; --same as DELETE eout WHERE--If you truncate a table, the counters on any identity columns will be reset.
--you can roll back the DELETE but not truncated table(unless u truncated after starting a transaction as "BEGIN TRAN;")

-------------
select TOP 50 PERCENT contact_ID  AS "NewCol"  INTO newTbl from xxx where pcd1_value liKE 'insur[ae]nce';--creates a new tbl
select * from newTbl;
drop table newTbl;
-------------
--to combine the result sets of 2 or more SELECT statements. UNION removes duplicate rows UNION ALL doesn't.
--must be same number of expressions in both SELECT statements. & matching dataTypes(DT)
--INTERSECT returns common records bw 2 select queries
--EXCEPT =all from 1st select that are NOT returned by 2nd query(common & unCommon) In oracle="MINUS"
SELECT product_id FROM products 
UNION / INTERSECT / EXCEPT 
SELECT product_id FROM inventory;
--
SELECT contact_id, contact_name FROM contacts WHERE site_name = 'TechOnTheNet.com'
UNION
SELECT company_id, company_name FROM companies WHERE site_name = 'CheckYourMath.com'
ORDER BY 2;--2nd col in resultSet(RS)
-------------
--PIVOT rotates col into row:  cross-tabulation . Sample tbl employees & departments created below.
--https://www.techonthenet.com/sql_server/pivot.php
select * from employees;
select * from departments;
--(1)write a source tbl = from(SELECT...)
--(2)tell an aggregate fn (SUM) to be applied to sourceTbl's col (dept_id)
--(3)write select with cols(30 & 45) that match the aggregate-group's key, output by pivot.
select 'colValue' as colHeader, [30] as 'dept#30', [45] as 'dept#45'
from (select dept_id, salary from employees) as srcTbl --tbl alias r must!! else err. WHY?
PIVOT (sum(salary) FOR dept_id IN([30], [45])) as pvtTbl;

select * from employees where dept_id in(30, 45);
-------------
--Enable/diaable a constraint: CHECK/NOCHECK before the keyWord constraint<->if after the word constraint then "CHECK" itself is one of the constraints e.g. CONSTRANT CHECK(employee_id BETWEEN 1 and 10000)=not on VIEW, can't have innerQuery, can't check non-self cols.
--WITH CHECK=chk older data against new rule being added/re-enabled. e.g. WITH CHECK CHECK constraint fk_emp_id=this enables FK and chks existing data against new contraint as well. 
--WITH NOCHECK=doesn't chk alrady existing data against new rule. e.g. WITH NOCHECK CHECK constraint unique_firstName.
--when add a constraint, default behaviour is of WITH-CHECK ADD. If at add time we do WITH NOCHECK then rule is considered disabled until explicitly enabled like  'ALTER TABLE WITH CHECK CHECK CONSTRAINT ALL".... http://stackoverflow.com/questions/17995793/what-does-second-check-constraint-mean
--{ CHECK | NOCHECK } CONSTRAINT = enable/disable FK or CHECK constraints only.
ALTER TABLE table_name
CHECK CONSTRAINT fk_name;
--

--


-------------
-------------
-------------
-------------
-------------
-------------
-------------
-------------
-------------
-------------
go --It means that all T-SQL prior to it will execute "at once". The GO command is used to group SQL commands into batches which are sent to the server together. The set of commands since the last GO  --http://stackoverflow.com/questions/2299249/what-is-the-use-of-go-in-sql-server-management-studio-transact-sql

--INSERT INTO mytable DEFAULT VALUES GO 10--will insert 10 rows i.e. will run related cmds 'n' # of times
-------------
select 1;  --just displays 1
-------------



--------------
--count(*)


--------------

----Syntax
--INSERT [ TOP (top_value) [ PERCENT ] ] ///DELETE [ TOP (top_value) [ PERCENT ] ]--top(10) percent of rows from subQuery
--INTO table
--(column1, column2, ... )
--SELECT expression1, expression2, ...
--FROM source_table
--[WHERE conditions];

---------------------------------------------------------------------------
CREATE TABLE DUAL
(
DUMMY VARCHAR(1)
)
GO
INSERT INTO DUAL (DUMMY)
VALUES ('X')
GO
---------------------------------------------------------------------------
select * from dual;
go --clear result NO ; at the end else err
---------------------------------------------------------------------------
create table xxx
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
xxx(contact_id,audio_module_no,audio_ch_no,pcd1_ID,pcd1_value) 
values('9128328744020010000','471011','14029946','RG','insurence');
insert into 
xxx(contact_id,audio_module_no,audio_ch_no,pcd2_ID,pcd2_value) 
values('9128328744020010001','471012','14029943','call purpose','credit');
insert into 
xxx(contact_id,audio_module_no,audio_ch_no,pcd2_ID,pcd2_value) 
values('9128328744020010003','471010','14029947','call purpose','debit');
insert into 
xxx(contact_id,audio_module_no,audio_ch_no,pcd2_ID,pcd2_value) 
values('9128328744020010004','471015','329947','call purpose','loan');


alter table xxx add 
start_time datetime,
end_time datetime;

--http://stackoverflow.com/questions/12957635/sql-query-to-insert-datetime-in-sql-server
--implicit casting of varchar to datetime etc: https://msdn.microsoft.com/en-us/library/ms187928.aspx
UPDATE xxx set start_time='20170101 10:34:09 AM', end_time='20170101 10:36:09 AM' where contact_id = '9128328744020010000';
UPDATE xxx set start_time='20170110 11:59:09 AM', end_time='20170110 12:04:09 PM' where contact_id = '9128328744020010001';
UPDATE xxx set start_time='20170115 11:58:09 PM', end_time='20170116 01:02:09 AM' where contact_id = '9128328744020010003';
UPDATE xxx set start_time='20170120 10:01:09 AM', end_time='20170120 10:07:09 AM' where contact_id = '9128328744020010004';

update xxx set pcd1_value = 'insurance' where contact_ID = 9128328744020010004;
---------------------------------------------------------------------------
--DDL/DML for practicing "PIVOT"
CREATE TABLE departments
( dept_id INT NOT NULL,
  dept_name VARCHAR(50) NOT NULL,
  CONSTRAINT departments_pk PRIMARY KEY (dept_id)
);

CREATE TABLE employees
( employee_number INT NOT NULL,
  last_name VARCHAR(50) NOT NULL,
  first_name VARCHAR(50) NOT NULL,
  salary INT,
  dept_id INT,
  CONSTRAINT employees_pk PRIMARY KEY (employee_number)
);

INSERT INTO departments
(dept_id, dept_name)
VALUES
(30, 'Accounting');

INSERT INTO departments
(dept_id, dept_name)
VALUES
(45, 'Sales');

INSERT INTO employees
(employee_number, last_name, first_name, salary, dept_id)
VALUES
(12009, 'Sutherland', 'Barbara', 54000, 45);

INSERT INTO employees
(employee_number, last_name, first_name, salary, dept_id)
VALUES
(34974, 'Yates', 'Fred', 80000, 45);

INSERT INTO employees
(employee_number, last_name, first_name, salary, dept_id)
VALUES
(34987, 'Erickson', 'Neil', 42000, 45);

INSERT INTO employees
(employee_number, last_name, first_name, salary, dept_id)
VALUES
(45001, 'Parker', 'Salary', 57500, 30);

INSERT INTO employees
(employee_number, last_name, first_name, salary, dept_id)
VALUES
(75623, 'Gates', 'Steve', 65000, 30);