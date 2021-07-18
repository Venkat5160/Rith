CREATE TABLE [dbo].[ClientInfo] (
    [Id]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [Info]      NVARCHAR (MAX) NULL,
    [AddedDate] DATETIME       NOT NULL,
    CONSTRAINT [PK_ClientInfo] PRIMARY KEY CLUSTERED ([Id] ASC)
);

