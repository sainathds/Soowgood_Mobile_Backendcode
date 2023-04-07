
SET IDENTITY_INSERT [dbo].[ProviderCategoryInfo] ON 

GO
INSERT [dbo].[ProviderCategoryInfo] ([Id], [MedicalCareType], [ProviderType], [Provider]) VALUES (3, N'Health-Care Professional', N'Doctors', N'Eye Specialist')
GO
INSERT [dbo].[ProviderCategoryInfo] ([Id], [MedicalCareType], [ProviderType], [Provider]) VALUES (4, N'Health-Care Professional', N'Doctors', N'Cancer Specialist')
GO
SET IDENTITY_INSERT [dbo].[ProviderCategoryInfo] OFF
GO
