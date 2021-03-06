/****** Object:  UserDefinedFunction [dbo].[gf_toNational]    Script Date: 01/18/2018 16:33:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gf_toNational]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[gf_toNational]
GO
/****** Object:  StoredProcedure [dbo].[sp_PageGetCommNew]    Script Date: 01/18/2018 16:33:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_PageGetCommNew]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_PageGetCommNew]
GO
/****** Object:  UserDefinedFunction [dbo].[toUppercaseRMB]    Script Date: 01/18/2018 16:33:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[toUppercaseRMB]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[toUppercaseRMB]
GO
/****** Object:  UserDefinedFunction [dbo].[toUppercaseRMB]    Script Date: 01/18/2018 16:33:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[toUppercaseRMB]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE function [dbo].[toUppercaseRMB] ( @LowerMoney decimal(18,4))  
returns varchar(200)   
as      
begin      
   declare @lowerStr varchar(200)      
   declare @UpperStr varchar(200)      
   declare @UpperPart varchar(200)     --长度      
   declare @i int       
         
   set @lowerStr=ltrim(rtrim(convert(decimal(18,2),round(@LowerMoney,2))))      
   set @i=1      
   set @UpperStr=''''      
         
   while(@i<=len(@lowerStr))      
   begin      
        select @UpperPart=  
        case substring(@lowerStr,len(@lowerStr)-@i+1,1)--取最后一位数  
            when  ''.'' then ''元''      
            when  ''0'' then ''零''      
            when  ''1'' then ''壹''      
            when  ''2'' then ''贰''      
            when  ''3'' then ''叁''      
            when  ''4'' then ''肆''      
            when  ''5'' then ''伍''      
            when  ''6'' then ''陆''      
            when  ''7'' then ''柒''      
            when  ''8'' then ''捌''      
            when  ''9'' then ''玖''      
        end      
        +      
        case @i       
            when 1 then  ''分''      
            when 2 then  ''角''      
            when 3 then  ''''      
            when 4 then  ''''      
            when 5 then  ''拾''      
            when 6 then  ''佰''      
            when 7 then  ''仟''      
            when 8 then  ''万''      
            when 9 then  ''拾''      
            when 10 then ''佰''      
            when 11 then ''仟''      
            when 12 then ''亿''      
            when 13 then ''拾''      
            when 14 then ''佰''      
            when 15 then ''仟''      
            when 16 then ''万''      
            else ''''      
        end      
        set @UpperStr=@UpperPart+@UpperStr      
        set @i=@i+1      
    end       
    set @UpperStr = REPLACE(@UpperStr,''零拾'',''零'')       
    set @UpperStr = REPLACE(@UpperStr,''零佰'',''零'')       
    set @UpperStr = REPLACE(@UpperStr,''零仟零佰零拾'',''零'')       
    set @UpperStr  = REPLACE(@UpperStr,''零仟'',''零'')      
    set @UpperStr = REPLACE(@UpperStr,''零零零'',''零'')      
    set @UpperStr = REPLACE(@UpperStr,''零零'',''零'')      
    set @UpperStr = REPLACE(@UpperStr,''零角零分'','''')      
    set @UpperStr = REPLACE(@UpperStr,''零分'','''')      
    set @UpperStr = REPLACE(@UpperStr,''零角'',''零'')      
    set @UpperStr = REPLACE(@UpperStr,''零亿零万零元'',''亿元'')      
    set @UpperStr = REPLACE(@UpperStr,''亿零万零元'',''亿元'')      
    set @UpperStr = REPLACE(@UpperStr,''零亿零万'',''亿'')      
    set @UpperStr = REPLACE(@UpperStr,''零万零元'',''万元'')      
    set @UpperStr = REPLACE(@UpperStr,''万零元'',''万元'')      
    set @UpperStr = REPLACE(@UpperStr,''零亿'',''亿'')      
    set @UpperStr = REPLACE(@UpperStr,''零万'',''万'')      
    set @UpperStr = REPLACE(@UpperStr,''零元'',''元'')      
    set @UpperStr = REPLACE(@UpperStr,''零零'',''零'')      
    if left(@UpperStr,1)=''元''      
        set @UpperStr = REPLACE(@UpperStr,''元'',''零元'')      
  
  return @UpperStr+''整''      
end      
' 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_PageGetCommNew]    Script Date: 01/18/2018 16:33:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_PageGetCommNew]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'





create PROC [dbo].[sp_PageGetCommNew]
@tbname     nvarchar(1000),           --要分页显示的表名
@FieldKey   nvarchar(1000),  --用于定位记录的主键(惟一键)字段,可以是逗号分隔的多个字段
@PageCurrent int=1,           --要显示的页码
@PageSize   int=10,            --每页的大小(记录数)
@FieldShow nvarchar(1000)='''',  --以逗号分隔的要显示的字段列表,如果不指定,则显示所有字段
@FieldOrder nvarchar(1000)='''',  --以逗号分隔的排序字段列表,可以指定在字段后面指定DESC/ASC 用于指定排序顺序
@Where    nvarchar(1000)='''', --查询条件
@PageCount int OUTPUT ,        --总页数
@RowCount int OUTPUT         --总行数
AS
SET NOCOUNT ON
--检查对象是否有效
--IF OBJECT_ID(@tbname) IS NULL
--BEGIN
--RAISERROR(N''对象"%s"不存在'',1,16,@tbname)
--RETURN
--END
--IF OBJECTPROPERTY(OBJECT_ID(@tbname),N''IsTable'')=0
--AND OBJECTPROPERTY(OBJECT_ID(@tbname),N''IsView'')=0
--AND OBJECTPROPERTY(OBJECT_ID(@tbname),N''IsTableFunction'')=0
--BEGIN
--RAISERROR(N''"%s"不是表、视图或者表值函数'',1,16,@tbname)
--RETURN
--END

--分页字段检查
IF ISNULL(@FieldKey,N'''')=''''
BEGIN
RAISERROR(N''分页处理需要主键（或者惟一键）'',1,16)
RETURN
END

--其他参数检查及规范
IF ISNULL(@PageCurrent,0)<1 SET @PageCurrent=1
IF ISNULL(@PageSize,0)<1 SET @PageSize=10
IF ISNULL(@FieldShow,N'''')=N'''' SET @FieldShow=N''*''
IF ISNULL(@FieldOrder,N'''')=N''''
SET @FieldOrder=N''''
ELSE
SET @FieldOrder=N''ORDER BY ''+LTRIM(@FieldOrder)
IF ISNULL(@Where,N'''')=N''''
SET @Where=N''''
ELSE
SET @Where=N''WHERE (''+@Where+N'')''

--如果@PageCount为NULL值,则计算总页数(这样设计可以只在第一次计算总页数,以后调用时,把总页数传回给存储过程,避免再次计算总页数,对于不想计算总页数的处理而言,可以给@PageCount赋值)
IF @PageCount IS NULL or @PageCount=0
BEGIN
DECLARE @sql nvarchar(4000)
SET @sql=N''SELECT @PageCount=COUNT(*)''
+N'' FROM ''+@tbname
+N'' ''+@Where
EXEC sp_executesql @sql,N''@PageCount int OUTPUT'',@PageCount OUTPUT
SET @RowCount=@PageCount
SET @PageCount=(@PageCount+@PageSize-1)/@PageSize
END

--计算分页显示的TOPN值
DECLARE @TopN varchar(20),@TopN1 varchar(20)
SELECT @TopN=@PageSize,
@TopN1=(@PageCurrent-1)*@PageSize

--第一页直接显示
IF @PageCurrent=1
EXEC(N''SELECT TOP ''+@TopN
+N'' ''+@FieldShow
+N'' FROM ''+@tbname
+N'' ''+@Where
+N'' ''+@FieldOrder)
ELSE
BEGIN
--处理别名


--执行查询
declare @strSQL   varchar(8000)

set @strSQL=(N''Select * FROM (Select ROW_NUMBER() OVER (''
+N'' ''+@FieldOrder
+N'' ) AS pos,''
+N'' ''+@FieldShow
+N'' FROM ''
+N'' ''+@tbname
+N'' ''+@Where
+N'' ) AS sp Where pos BETWEEN ''
+N'' ''+str((@PageCurrent-1)*@PageSize+1) 
+N'' AND ''
+N'' ''+str(@PageCurrent*@PageSize))

print @strSQL
EXEC (@strSQL)
end











' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[gf_toNational]    Script Date: 01/18/2018 16:33:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gf_toNational]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'--drop function gf_toNational
CREATE function [dbo].[gf_toNational](
	@as_teeth nvarchar(200),
	@as_pos nvarchar(200)
)
returns nvarchar(1000)

begin
	declare @ls_teeth nvarchar(1000)
	declare @ls_first nvarchar(1000)
	declare @ls_sec nvarchar(1000)
	declare @ls_third nvarchar(1000)
	declare @ls_express nvarchar(1000)
	declare @ls_link nvarchar(1000)
	declare @ls_part nvarchar(1000)
	declare @ls_result nvarchar(1000)
	declare @i int
	declare @li_temp1 int
	declare @li_temp2 int
	set @ls_teeth = @as_teeth
	set @ls_express=''''
	set @ls_link=''''
	if len(@ls_teeth)>1 
		begin
			--将连续的单冠转换
			set @ls_first = left(@ls_teeth,1) 
			set @ls_teeth = substring(@ls_teeth,2,len(@ls_teeth))
			set @i=0
		
		while len(@ls_teeth)>0
			begin
				set @ls_sec = substring(@ls_teeth,@i+1,1)
				set @ls_third = substring(@ls_teeth,@i+2,1)
			if isnumeric(@ls_sec)<>0 and isnumeric(@ls_third)<>0  
				begin
					if @as_pos=''A'' or @as_pos=''C''
						--右牙位
						begin
						set @li_temp1 = cast(@ls_first as int) - @i - 1
						set @li_temp2 = cast(@ls_sec as int) - 1
						end
					else
						--左牙位
						begin
						set @li_temp1 = cast(@ls_first as int) + @i + 1
						set @li_temp2 = cast(@ls_sec as int) + 1
						end
					if @li_temp1 = cast(@ls_sec as int) 
						if @li_temp2 = cast(@ls_third as int)
							begin   
							set @i+=1
							set @ls_link = '',-,''
							end
						else
							begin
								if right(@ls_express,1) = @ls_first
									begin 	
										set @ls_express += @ls_link+@ls_sec + @ls_third
									end
								else
									begin
										set @ls_express += @ls_first +@ls_link+@ls_sec + @ls_third
									end
								
								set @ls_teeth = substring(@ls_teeth,@i+3,len(@ls_teeth))
								set @ls_first = @ls_third
								set @ls_link = ''''
								set @i=0
							end
					else
						begin
						if right(@ls_express,1) = @ls_first 
							begin
							set @ls_express += @ls_link+@ls_sec
							end
						else
							begin
							set @ls_express += @ls_first +@ls_link+@ls_sec
							end
						set @ls_teeth = substring(@ls_teeth,@i+2,len(@ls_teeth))
						set @ls_first = @ls_sec
						set @ls_link = ''''
						set @i=0
						end
					end
			else
				begin
					if right(@ls_express,1) = @ls_first
						begin
						set @ls_express += @ls_link+@ls_sec+@ls_third
						set @ls_link = ''''
						set @ls_teeth = substring(@ls_teeth,@i+2,len(@ls_teeth))
						set @i=1
						set @ls_first =substring(@ls_teeth,@i,1)
						end
					else
						begin
						if @ls_first = @ls_sec
							begin 
							set @ls_express += @ls_first +@ls_link+@ls_third
							set @ls_link = ''''
							set @ls_teeth = substring(@ls_teeth,@i+2,len(@ls_teeth))
							set @i=1
							set @ls_first = substring(@ls_teeth,@i,1)
							end
						else
							begin
							set @ls_express += @ls_first +@ls_link+@ls_sec+@ls_third
							set @ls_link = ''''
							set @ls_first = @ls_third
							end
							if @i+2 >= len(@ls_teeth) 
								begin
								set @ls_teeth = ''''
								end
							else
								begin
								set @ls_teeth = substring(@ls_teeth,@i+3,len(@ls_teeth))
								end
						set @i=0
						end
				end
			end
	end
else
	set @ls_express = @ls_teeth


----国际表示法
   set @ls_result = ''''
	if @as_pos = ''A'' 
		begin
		--//上右牙位
		while len(@ls_express)>0
			begin
				set @ls_part = left(@ls_express,1)
				set @ls_express = substring(@ls_express,@i+2,len(@ls_express))
				if @ls_part = ''1''
						set @ls_result +=  ''11''
				if @ls_part = ''2''
						set @ls_result +=  ''12''
				if @ls_part = ''3''
						set @ls_result +=  ''13''
				if @ls_part = ''4''
						set @ls_result +=  ''14''
				if @ls_part = ''5''
						set @ls_result +=  ''15''
				if @ls_part = ''6''
						set @ls_result += ''16''
				if @ls_part = ''7''
						set @ls_result +=  ''17''
				if @ls_part = ''8''
						set @ls_result +=  ''18''
				else
					set @ls_result +=  @ls_part
			end
		end
		
	if @as_pos = ''B'' 
		--//上左牙位
		 while len(@ls_express)>0
			begin
			set @ls_part = left(@ls_express,1)
			set @ls_express = substring(@ls_express,@i+2,len(@ls_express))
			if @ls_part = ''1''
						set @ls_result +=  ''21''
				if @ls_part = ''2''
						set @ls_result +=  ''22''
				if @ls_part = ''3''
						set @ls_result +=  ''23''
				if @ls_part = ''4''
						set @ls_result +=  ''24''
				if @ls_part = ''5''
						set @ls_result +=  ''25''
				if @ls_part = ''6''
						set @ls_result += ''26''
				if @ls_part = ''7''
						set @ls_result +=  ''27''
				if @ls_part = ''8''
						set @ls_result +=  ''28''
				else
					set @ls_result +=  @ls_part
			end

	if @as_pos = ''C''
		--//下右牙位
		while len(@ls_express)>0
			begin
			set @ls_part = left(@ls_express,1)
			set @ls_express = substring(@ls_express,@i+2,len(@ls_express))
			
				if @ls_part = ''1''
						set @ls_result +=  ''41''
				if @ls_part = ''2''
						set @ls_result +=  ''42''
				if @ls_part = ''3''
						set @ls_result +=  ''43''
				if @ls_part = ''4''
						set @ls_result +=  ''44''
				if @ls_part = ''5''
						set @ls_result +=  ''45''
				if @ls_part = ''6''
						set @ls_result += ''46''
				if @ls_part = ''7''
						set @ls_result +=  ''47''
				if @ls_part = ''8''
						set @ls_result +=  ''48''
				else
					set @ls_result +=  @ls_part
		end
	if @as_pos = ''D''
		--//下左牙位
		 while len(@ls_express)>0
			begin
			set @ls_part = left(@ls_express,1)
			set @ls_express = substring(@ls_express,@i+2,len(@ls_express))
			
				if @ls_part = ''1''
						set @ls_result +=  ''31''
				if @ls_part = ''2''
						set @ls_result +=  ''32''
				if @ls_part = ''3''
						set @ls_result +=  ''33''
				if @ls_part = ''4''
						set @ls_result +=  ''34''
				if @ls_part = ''5''
						set @ls_result +=  ''35''
				if @ls_part = ''6''
						set @ls_result += ''36''
				if @ls_part = ''7''
						set @ls_result +=  ''37''
				if @ls_part = ''8''
						set @ls_result +=  ''38''
			end
return @ls_result
end' 
END
GO
