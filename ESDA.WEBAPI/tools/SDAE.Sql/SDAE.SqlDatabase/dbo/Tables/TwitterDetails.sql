CREATE TABLE [dbo].[TwitterDetails] (
    [TwitterId]    INT           IDENTITY (1, 1) NOT NULL,
    [Username]     VARCHAR (200) NULL,
    [Password]     VARCHAR (200) NULL,
    [Url]          VARCHAR (200) NULL,
    [ClientId]     VARCHAR (200) NULL,
    [SecretKey]    VARCHAR (200) NULL,
    [CreatedDate]  DATETIME      NOT NULL,
    [CreatedBy]    VARCHAR (100) NOT NULL,
    [ModifiedDate] DATETIME      NULL,
    [ModifiedBy]   VARCHAR (100) NULL,
    [StatusTypeId] TINYINT       NOT NULL,
    CONSTRAINT [PK_TwitterDetails] PRIMARY KEY CLUSTERED ([TwitterId] ASC),
    CONSTRAINT [FK_TwitterDetails_StatusTypes] FOREIGN KEY ([StatusTypeId]) REFERENCES [dbo].[StatusTypes] ([StatusTypeId])
);

