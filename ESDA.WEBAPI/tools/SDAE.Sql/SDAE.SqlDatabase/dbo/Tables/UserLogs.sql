CREATE TABLE [dbo].[UserLogs] (
    [UserLogId]         BIGINT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [UserName]          VARCHAR (50)     NOT NULL,
    [UserId]            NVARCHAR (450)   NULL,
    [ClientIp]          NVARCHAR (60)    NULL,
    [LogonTime]         DATETIME         NOT NULL,
    [LogoutTime]        DATETIME         NULL,
    [SessionId]         UNIQUEIDENTIFIER NOT NULL,
    [DeviceModel]       VARCHAR (200)    NULL,
    [DeviceToken]       VARCHAR (500)    NULL,
    [LoginDeviceTypeId] SMALLINT         NULL,
    [LoginStatusTypeId] INT              NULL,
    CONSTRAINT [PK_UserLogs] PRIMARY KEY CLUSTERED ([UserLogId] ASC),
    CONSTRAINT [FK_UserLogs_LoginStatusTypes] FOREIGN KEY ([LoginStatusTypeId]) REFERENCES [dbo].[LoginStatusTypes] ([LoginStatusTypeId])
);

