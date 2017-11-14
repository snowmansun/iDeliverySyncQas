
-- =============================================
-- Author:		<Younest>
-- Create date: <2012-05-21>
-- Description:	<Description>
-- Modified History
-- =============================================

CREATE PROCEDURE [dbo].[SelectInforDetails]
    @UserCode NVARCHAR(50) ,
    @ActionType INT ,
    @Level NVARCHAR(20) ,
    @StartTime DATETIME ,
    @EndTime DATETIME
AS 
    BEGIN
        SELECT  Id ,
                UserCode ,
                Date ,
                CONVERT(NVARCHAR(180), Message) Message
        FROM    dbo.SyncLog
        WHERE   UserCode = @UserCode
                AND ActionType = @ActionType
                AND Level = @Level
                AND date >= @StartTime
                AND date <= @EndTime
    END

GO


