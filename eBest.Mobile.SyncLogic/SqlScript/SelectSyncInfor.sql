
-- =============================================
-- Author:		<Younest>
-- Create date: <2012-05-15>
-- Description:	<Description>
-- Modified History
-- =============================================


CREATE PROCEDURE [dbo].[SelectSyncInfor]
    @UserCode NVARCHAR(50) ,
    @ActionType INT
AS 
    BEGIN
        SELECT  Id ,
                UserCode ,
                Date ,
                CONVERT(NVARCHAR(180), Message) Message
        FROM    dbo.SyncLog
        WHERE   UserCode = @UserCode
                AND ActionType = @ActionType
                AND date >= DATEADD(mi, -1, GETDATE()) 
    END





GO


