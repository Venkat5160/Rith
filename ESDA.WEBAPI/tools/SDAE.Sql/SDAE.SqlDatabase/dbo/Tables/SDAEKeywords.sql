CREATE TABLE [dbo].[SDAEKeywords] (
    [SDAEKeywordId] INT             IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (1000) NOT NULL,
    [CreatedDate]   DATETIME        NOT NULL,
    [CreatedBy]     VARCHAR (100)   NOT NULL,
    [ModifiedDate]  DATETIME        NULL,
    [ModifiedBy]    VARCHAR (100)   NULL,
    [StatusTypeId]  TINYINT         NOT NULL,
    CONSTRAINT [PK_SDAEKeywords] PRIMARY KEY CLUSTERED ([SDAEKeywordId] ASC),
    CONSTRAINT [FK_SDAEKeywords_StatusTypes] FOREIGN KEY ([StatusTypeId]) REFERENCES [dbo].[StatusTypes] ([StatusTypeId])
);

