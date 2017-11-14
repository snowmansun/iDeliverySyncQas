-- ==========================================================================================
-- Entity Name:	SyncUpload_CRASHLOG
-- Author:	eBestMobile
-- Create date:	2012-09-24 10:45:06
-- Description:	This stored procedure is intended for selecting all rows from CRASHLOG table
-- ==========================================================================================
CREATE PROCEDURE [dbo].[SyncUpload_CRASHLOG]
    @USER_ID VARCHAR(20) ,
    @CUSTOMER_ID INT ,
    @Date DATETIME ,
    @LOGINFO VARCHAR(MAX)
AS 
    BEGIN
        INSERT  INTO CRASHLOG
                ( [USER_ID] ,
                  [CUSTOMER_ID] ,
                  [DATE] ,
                  [LOGINFO]
                )
        VALUES  ( @USER_ID ,
                  @CUSTOMER_ID ,
                  @Date ,
                  @LOGINFO 
                )

    END


GO


