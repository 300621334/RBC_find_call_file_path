--SearchAllTables "Finance";--Have to "new query" on EmployeeDatabase, then run this search
SearchAllTables "Levinson"; --in EmployeeDatabase

--drop proc SearchAllTables;

-------------------------------------------------
--procedure is below
-------------------------------------------------
----searches ALL tables in a database.
----pound sign prefix for temporary tables: http://stackoverflow.com/questions/3166117/what-does-the-sql-symbol-mean-and-how-is-it-used
--/*The pound sign # is used to prefix temporary tables and procedures. A single instance (#) refers to a temporary object that lives/dies with the current session while a double instance (##) is a global object.*/


----run the procedure in "EmployeeDatabase" once then use it like so SearchAllTables "searchWord". All fields r converted to string. http://stackoverflow.com/questions/15757263/find-a-string-by-searching-all-tables-in-sql-server-management-studio-2008
----SearchAllTables "Finance";
----SearchAllTables "Levinson";


--CREATE PROC SearchAllTables
--(
--    @SearchStr nvarchar(100)
--)
--AS
--BEGIN --1st BEGIN

--    CREATE TABLE #Results (ColumnName nvarchar(370), ColumnValue nvarchar(3630)) --# means temporary table that lives ONLY inside this BEGIN block

--    SET NOCOUNT ON --for performance. Skip info like "4 rows affected" etc!!! hence less traffic on net.

--    DECLARE @TableName nvarchar(256), @ColumnName nvarchar(128), @SearchStr2 nvarchar(110)
--    SET  @TableName = '' --so that it is NOT NULL and while() runs at least once
--    SET @SearchStr2 = QUOTENAME('%' + @SearchStr + '%','''')--To prevent injection attack via search phrase, quote that phrase so it becomes a literal. Will put single quotes around catenated str '%@SearchStr%'. If '''' not specified, then surrounds by [%@SearchStr%] as a default. If str is longer than 128chars then it returns NULL. http://www.sql-server-performance.com/2014/using-quotename-protect-sqlinjection/


--    WHILE @TableName IS NOT NULL  --loop the 2nd BEGIN block till no more tables left
--    BEGIN --2nd BEGIN
--        SET @ColumnName = '' --assign colName a value that is smallest of all existing col names but still not NULL
--        SET @TableName = --set TableName to one step larger than empty string ''.
--        (
--            SELECT MIN(QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME))
--            FROM     INFORMATION_SCHEMA.TABLES                --a view, located under Views > System Views
--            WHERE         TABLE_TYPE = 'BASE TABLE' --2 values of TABLE_TYPE are "VIEW" or "BASE TABLE"
--                AND    QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME) > @TableName --one larger than existing name. in start TableName = '' i.e. smallest of all names
--                AND    OBJECTPROPERTY(      
--                        OBJECT_ID(
--                            QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME)
--                             ), 'IsMSShipped' --it is "property". 2nd param in  OBJECTPROPERTY ( idOfObj:int , property ). Returns an int like we chk if it's 'IsMsShipped'=0  i.e. not a obj/table created when SqlServer was installed.
--                               ) = 0  --OBJECTPROPERTY=0 --Whether Object created during installation of SQL Server? >> 1 = True  0 = False
--        )

		

--        WHILE (@TableName IS NOT NULL) AND (@ColumnName IS NOT NULL) --loop the 3rd BEGIN blk untill no more columns left
--        BEGIN --3rd BEGIN
--            SET @ColumnName =
--            (
--                SELECT MIN(QUOTENAME(COLUMN_NAME))
--                FROM     INFORMATION_SCHEMA.COLUMNS
--                WHERE         TABLE_SCHEMA    = PARSENAME(@TableName, 2)--extract a piece from whole objName: 1 = Object name, 2 = Schema name, 3 = Database name, 4 = Server name
--                    AND    TABLE_NAME    = PARSENAME(@TableName, 1)
--                    AND    DATA_TYPE IN ('char', 'varchar', 'nchar', 'nvarchar', 'int', 'decimal')
--                    AND    QUOTENAME(COLUMN_NAME) > @ColumnName
--            )

--            IF @ColumnName IS NOT NULL --in start colName was '' then we assigned a colName from table. It there WAS indeed some col(s) in table then proceed e 4th BEGIN
--            BEGIN --4rd BEGIN
--                INSERT INTO #Results
--                EXEC  --LEFT() Returns left most 3630 chars from a str(value of that column). --NOLOCK or newer WITH(NOLOCK) allows to read an inserted row that's not committed yet.Bcoz it doesn't LOCK the table hence allowing another query to insert any row(s). Don't put comments inside EEC() as they don't appear to be comments!!
--                (
--                    'SELECT ''' + @TableName + '.' + @ColumnName + ''', LEFT(' + @ColumnName + ', 3630)
--                    FROM ' + @TableName + ' (NOLOCK) ' +
--                    ' WHERE ' + @ColumnName + ' LIKE ' + @SearchStr2
--                )
--            END --4rd BEGIN ends
--        END --3nd BEGIN ends   
--    END --2st BEGIN ends

--    SELECT ColumnName, ColumnValue FROM #Results
--END --1st BEGIN ends. PlSql block ends