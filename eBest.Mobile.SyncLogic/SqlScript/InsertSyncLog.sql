

CREATE PROCEDURE [dbo].[InsertSyncLog]
    @UserCode NVARCHAR(50) ,
    @action_type INT ,
    @log_date DATETIME ,
    @thread VARCHAR ,
    @log_level VARCHAR(50) ,
    @logger VARCHAR(255) ,
    @message NTEXT ,
    @exception VARCHAR(2000)
AS 
    BEGIN
        INSERT  INTO SyncLog
                ( UserCode ,
                  ActionType ,
                  Date ,
                  Thread ,
                  Level ,
                  Logger ,
                  Message ,
                  Exception
                )
        VALUES  ( @UserCode ,
                  @action_type ,
                  @log_date ,
                  @thread ,
                  @log_level ,
                  @logger ,
                  @message ,
                  @exception
                )
    END

GO


