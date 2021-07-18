CREATE TABLE [dbo].[StatusTypes] (
    [StatusTypeId] TINYINT      NOT NULL,
    [Name]         VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_StatusTypes] PRIMARY KEY CLUSTERED ([StatusTypeId] ASC)
);

