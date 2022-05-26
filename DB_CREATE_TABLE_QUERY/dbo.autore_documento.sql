CREATE TABLE [dbo].[autore_documento] (
    [id_autore]    INT NOT NULL,
    [id_documento] INT NOT NULL,
    CONSTRAINT [multiple_primary] PRIMARY KEY CLUSTERED ([id_autore] ASC, [id_documento] ASC),
    CONSTRAINT [ref_autore] FOREIGN KEY ([id_autore]) REFERENCES [dbo].[autori] ([id_autore]),
    CONSTRAINT [ref_documento] FOREIGN KEY ([id_documento]) REFERENCES [dbo].[documenti] ([codice])
);

