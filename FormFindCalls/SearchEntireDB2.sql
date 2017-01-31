select * from sys.all_objects x inner join sys.all_columns y on x.object_id=y.object_id where x.type_desc='USER_TABLE' and x.object_id >0 ;--negative object_id for system tables that still are declared as "USER_TABLES" for some reason
Select Object_ID('employee');--to get object ID of a tbl etc. UserDefined obj & sys tables have positive ID while others have negative obj_id
select * FROM     INFORMATION_SCHEMA.TABLES;
select * from trace_xe_action_map;

drop procedure SearchAllTables;
drop proc SearchAllTables2;


SearchAllTables2 '(555)555-5555';--Have to "new query" on EmployeeDatabase, then run this search
SearchAllTables2 "john"; --case inSensitive --in EmployeeDatabase
SearchAllTables2 "ce";
--select * FROM sys.all_objects order by type_desc;
select * FROM sys.all_columns order by object_id;
select *  FROM     INFORMATION_SCHEMA.views;
select *  FROM employee;




-----------------------------------------------
procedure is below
-----------------------------------------------
--searches ALL tables in a database.
--pound sign prefix for temporary tables: http://stackoverflow.com/questions/3166117/what-does-the-sql-symbol-mean-and-how-is-it-used
/*The pound sign # is used to prefix temporary tables and procedures. A single instance (#) refers to a temporary object that lives/dies with the current session while a double instance (##) is a global object.*/


--run the procedure in "EmployeeDatabase" once then use it like so SearchAllTables "searchWord". All fields r converted to string. http://stackoverflow.com/questions/15757263/find-a-string-by-searching-all-tables-in-sql-server-management-studio-2008
--SearchAllTables2 "Finance";
--SearchAllTables2 "Levinson";


CREATE PROC SearchAllTables2
(
    @SearchStr nvarchar(100)
)
AS
BEGIN --1st BEGIN

    CREATE TABLE #Results (ColumnName nvarchar(370), ColumnValue nvarchar(3630)) --# means temporary table that lives ONLY inside this BEGIN block

    SET NOCOUNT ON --for performance. Skip info like "4 rows affected" etc!!! hence less traffic on net.

    DECLARE @TableName nvarchar(256), @ColumnName nvarchar(128), @SearchStr2 nvarchar(110)
    SET  @TableName = '' --so that it is NOT NULL and while() runs at least once
    SET @SearchStr2 = QUOTENAME('%' + @SearchStr + '%','''')--To prevent injection attack via search phrase, quote that phrase so it becomes a literal. Will put single quotes around catenated str '%@SearchStr%'. If '''' not specified, then surrounds by [%@SearchStr%] as a default. If str is longer than 128chars then it returns NULL. http://www.sql-server-performance.com/2014/using-quotename-protect-sqlinjection/


    WHILE @TableName IS NOT NULL  --loop the 2nd BEGIN block till no more tables left
    BEGIN --2nd BEGIN
        SET @ColumnName = '' --assign colName a value that is smallest of all existing col names but still not NULL
        SET @TableName = --set TableName to one step larger than empty string ''.
        (
            SELECT MIN(QUOTENAME(x.name))
			from sys.all_objects x
			where x.type_desc='USER_TABLE' and x.object_id >0 
            AND    QUOTENAME(x.name) > @TableName --one larger than existing name. in start TableName = '' i.e. smallest of all names
                
        )

		

        WHILE (@TableName IS NOT NULL) AND (@ColumnName IS NOT NULL) --loop the 3rd BEGIN blk untill no more columns left
        BEGIN --3rd BEGIN
            SET @ColumnName =
            (
                SELECT MIN(QUOTENAME(y.name))
                --FROM     INFORMATION_SCHEMA.COLUMNS
				from sys.all_objects x inner join sys.all_columns y on x.object_id=y.object_id
				where x.type_desc='USER_TABLE' and x.object_id >0 
                --WHERE         TABLE_SCHEMA    = PARSENAME(@TableName, 2)--extract a piece from whole objName: 1 = Object name, 2 = Schema name, 3 = Database name, 4 = Server name
                --    AND    TABLE_NAME    = PARSENAME(@TableName, 1)
                --    AND    DATA_TYPE IN ('char', 'varchar', 'nchar', 'nvarchar', 'int', 'decimal')
                    AND    QUOTENAME(y.name) > @ColumnName
					AND QUOTENAME(x.name) = @TableName --eout this cols will be searched in wrong tbls as well, giving err
            )

            IF @ColumnName IS NOT NULL --in start colName was '' then we assigned a colName from table. It there WAS indeed some col(s) in table then proceed e 4th BEGIN
            BEGIN --4rd BEGIN
                INSERT INTO #Results
                EXEC  --LEFT() Returns left most 3630 chars from a str(value of that column). --NOLOCK or newer WITH(NOLOCK) allows to read an inserted row that's not committed yet.Bcoz it doesn't LOCK the table hence allowing another query to insert any row(s). Don't put comments inside EEC() as they don't appear to be comments!!
                (
                    'SELECT ''' + @TableName + '.' + @ColumnName + ''', LEFT(' + @ColumnName + ', 3630)
                    FROM ' + @TableName + ' (NOLOCK) ' +
                    ' WHERE ' + @ColumnName + ' LIKE ' + @SearchStr2
                )
            END --4rd BEGIN ends
        END --3nd BEGIN ends   
    END --2st BEGIN ends

    SELECT ColumnName AS "Table.&.Column", ColumnValue FROM #Results
END --1st BEGIN ends. PlSql block ends