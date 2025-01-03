USE [TechImplementCRM]
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Account]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[AccountId] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_Account_AccountId]  DEFAULT (newid()),
	[Name] [nvarchar](50) NULL,
	[ShippingAddress] [uniqueidentifier] NULL,
	[BillingAddress] [uniqueidentifier] NULL,
	[AccountTypeId] [uniqueidentifier] NULL,
	[PhoneOffice] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[Fax] [nvarchar](50) NULL,
	[WebSite] [nvarchar](50) NULL,
	[AccountSizeId] [uniqueidentifier] NULL,
	[IndustryId] [uniqueidentifier] NULL,
	[Description] [nvarchar](500) NULL,
	[IsDeleted] [bit] NULL,
	[StatusId] [uniqueidentifier] NULL,
	[AssignedUser] [uniqueidentifier] NULL,
	[AssignedTeam] [uniqueidentifier] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AccountLeads]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountLeads](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[AccountId] [bigint] NULL,
	[LeadId] [bigint] NULL,
 CONSTRAINT [PK_AccountLeads] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AccountSize]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountSize](
	[AccountSizeId] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_AccountSize_AccountSizeId]  DEFAULT (newid()),
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_AccountSize] PRIMARY KEY CLUSTERED 
(
	[AccountSizeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AccountType]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountType](
	[AccountTypeId] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_AccountType_AccountTypeId]  DEFAULT (newid()),
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_AccountType] PRIMARY KEY CLUSTERED 
(
	[AccountTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Activity]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Activity](
	[ActivityId] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Name] [nvarchar](50) NULL,
	[ActivityPartyId] [uniqueidentifier] NULL,
	[ActivityPointerId] [uniqueidentifier] NULL,
	[Description] [nvarchar](500) NULL,
	[StatusId] [uniqueidentifier] NULL,
	[AssignedUser] [uniqueidentifier] NULL,
	[AssignedTeam] [uniqueidentifier] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Activity] PRIMARY KEY CLUSTERED 
(
	[ActivityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Address]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Address](
	[AddressId] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_Address_AddressId]  DEFAULT (newid()),
	[Street1] [nvarchar](100) NULL,
	[Street2] [nvarchar](100) NULL,
	[City] [nvarchar](20) NULL,
	[State] [nvarchar](20) NULL,
	[Zip] [nvarchar](15) NULL,
	[Country] [nvarchar](20) NULL,
 CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED 
(
	[AddressId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Alert]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Alert](
	[AlertId] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_Alert_AlertId]  DEFAULT (newid()),
	[Title] [nvarchar](50) NULL,
	[UrgencyId] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Alert_UrgencyId]  DEFAULT (newid()),
	[Description] [nvarchar](500) NULL,
	[IsDeleted] [bit] NULL,
	[StatusId] [uniqueidentifier] NULL,
	[AssignedUser] [uniqueidentifier] NULL,
	[AssignedTeam] [uniqueidentifier] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Alert] PRIMARY KEY CLUSTERED 
(
	[AlertId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](128) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Category]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[CategoryId] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Description] [nvarchar](500) NULL,
	[IsDeleted] [bit] NULL,
	[StatusId] [uniqueidentifier] NULL,
	[AssignedUser] [uniqueidentifier] NULL,
	[AssignedTeam] [uniqueidentifier] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Category_1] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Contact]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contact](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_tbl_contact] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ContactLeads]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContactLeads](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ContactId] [bigint] NOT NULL,
	[LeadId] [bigint] NOT NULL,
 CONSTRAINT [PK_ContactLeads] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ContactOrders]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContactOrders](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_ContactOrders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ContactQuotes]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContactQuotes](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_ContactQuotes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Currency]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Currency](
	[CurrencyId] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_Currency_CurrencyId]  DEFAULT (newid()),
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_Currency] PRIMARY KEY CLUSTERED 
(
	[CurrencyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CustomerAsset]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerAsset](
	[CustomerAssetId] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_CustomerAsset_CustomerAssetId]  DEFAULT (newid()),
	[Title] [nvarchar](50) NULL,
	[CustomerAssetTypeId] [uniqueidentifier] NULL,
	[Manufacture] [nvarchar](20) NULL,
	[Model] [nvarchar](20) NULL,
	[YearOfManufacture] [int] NULL,
	[Value] [decimal](18, 0) NULL,
	[DepriciatedValue] [decimal](18, 0) NULL,
	[SKU] [nvarchar](32) NULL,
	[Description] [nvarchar](500) NULL,
	[IsDeleted] [bit] NULL,
	[StatusId] [uniqueidentifier] NULL,
	[AssignedUser] [uniqueidentifier] NULL,
	[AssignedTeam] [uniqueidentifier] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_CustomerAsset] PRIMARY KEY CLUSTERED 
(
	[CustomerAssetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CustomerAssetType]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerAssetType](
	[CustomerAssetTypeId] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_CustomerAssetType_CustomerAssetTypeId]  DEFAULT (newid()),
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_CustomerAssetType] PRIMARY KEY CLUSTERED 
(
	[CustomerAssetTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DeciveConfiguration]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeciveConfiguration](
	[DeviceConfigurationId] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[DeviceId] [uniqueidentifier] NULL,
	[IsSleepModeEnabled] [bit] NULL,
	[SleepModeValueInSeconds] [int] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_DeciveConfiguration] PRIMARY KEY CLUSTERED 
(
	[DeviceConfigurationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Device]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Device](
	[DeviceId] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_Devices_DeviceId]  DEFAULT (newid()),
	[Name] [nvarchar](50) NULL,
	[Mac] [nvarchar](50) NULL,
	[EMEINumber] [nvarchar](50) NULL,
	[RegistrationDate] [datetime] NULL,
	[Latitude] [decimal](10, 8) NULL,
	[Longitude] [decimal](11, 8) NULL,
	[IsDeleted] [bit] NULL,
	[StatusId] [uniqueidentifier] NULL,
	[AssignedUser] [uniqueidentifier] NULL,
	[AssignedTeam] [uniqueidentifier] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Devices] PRIMARY KEY CLUSTERED 
(
	[DeviceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DeviceSensor]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeviceSensor](
	[DeviceSensorId] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[SensorName] [nvarchar](50) NULL,
	[DeviceId] [uniqueidentifier] NULL,
	[SensorType] [nvarchar](50) NULL,
	[StatusId] [uniqueidentifier] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_DeviceSensor] PRIMARY KEY CLUSTERED 
(
	[DeviceSensorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Email]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Email](
	[EmailId] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[ActivityId] [uniqueidentifier] NULL,
	[StatusId] [uniqueidentifier] NULL,
	[AssignedUser] [uniqueidentifier] NULL,
	[AssignedTeam] [uniqueidentifier] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Email] PRIMARY KEY CLUSTERED 
(
	[EmailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Industry]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Industry](
	[IndustryId] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_Industry_IndustryId]  DEFAULT (newid()),
	[Name] [nvarchar](100) NULL,
 CONSTRAINT [PK_Industry] PRIMARY KEY CLUSTERED 
(
	[IndustryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Lead]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lead](
	[LeadId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](100) NULL,
	[LeadTypeId] [uniqueidentifier] NULL,
	[LeadSourceId] [uniqueidentifier] NULL,
	[PhoneNumber] [nvarchar](15) NULL,
	[Email] [nvarchar](50) NULL,
	[AddressId] [uniqueidentifier] NULL,
	[IndustryId] [uniqueidentifier] NULL,
	[Description] [nvarchar](500) NULL,
	[IsDeleted] [bit] NULL,
	[StatusId] [uniqueidentifier] NULL,
	[AssignedUser] [uniqueidentifier] NULL,
	[AssignedTeam] [uniqueidentifier] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Lead] PRIMARY KEY CLUSTERED 
(
	[LeadId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LeadSource]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LeadSource](
	[LeadSourceId] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_LeadSource_LeadSourceId]  DEFAULT (newid()),
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_LeadSource] PRIMARY KEY CLUSTERED 
(
	[LeadSourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LeadType]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LeadType](
	[LeadTypeId] [uniqueidentifier] NOT NULL CONSTRAINT [DF_LeadType_LeadTypeId]  DEFAULT (newid()),
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_LeadType] PRIMARY KEY CLUSTERED 
(
	[LeadTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Location]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Location](
	[LocationId] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_Location_LocationId]  DEFAULT (newid()),
	[Name] [nvarchar](50) NULL,
	[AddressId] [uniqueidentifier] NULL,
	[LocationTypeId] [uniqueidentifier] NULL,
	[Description] [nvarchar](500) NULL,
	[Latitude] [decimal](10, 8) NULL,
	[Longitude] [decimal](11, 8) NULL,
	[IsDeleted] [bit] NULL,
	[StatusId] [uniqueidentifier] NULL,
	[AssignedUser] [uniqueidentifier] NULL,
	[AssignedTeam] [uniqueidentifier] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Location_1] PRIMARY KEY CLUSTERED 
(
	[LocationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LocationType]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LocationType](
	[LocationTypeId] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_LocationType_LocationTypeId]  DEFAULT (newid()),
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_LocationType] PRIMARY KEY CLUSTERED 
(
	[LocationTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Meeting]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Meeting](
	[MeetingId] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[ActivityId] [uniqueidentifier] NULL,
	[StatusId] [uniqueidentifier] NULL,
	[AssignedUser] [uniqueidentifier] NULL,
	[AssignedTeam] [uniqueidentifier] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Meeting] PRIMARY KEY CLUSTERED 
(
	[MeetingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Opportunity]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Opportunity](
	[OpportunityId] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_Oppertunity_OppertunityId]  DEFAULT (newid()),
	[Amount] [decimal](18, 0) NULL,
	[ProbabilityId] [uniqueidentifier] NULL,
	[OpportunityStageId] [uniqueidentifier] NULL,
	[Title] [nvarchar](50) NULL,
	[CurrencyId] [uniqueidentifier] NULL,
	[Description] [nvarchar](500) NULL,
	[IsDeleted] [bit] NULL,
	[StatusId] [uniqueidentifier] NULL,
	[AssignedUser] [uniqueidentifier] NULL,
	[AssignedTeam] [uniqueidentifier] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Opportunity] PRIMARY KEY CLUSTERED 
(
	[OpportunityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OpportunityStages]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OpportunityStages](
	[OpportunityStageId] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_OppertunityStages_OppertunityStageId]  DEFAULT (newid()),
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_OpportunityStages] PRIMARY KEY CLUSTERED 
(
	[OpportunityStageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PhoneCall]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhoneCall](
	[PhoneCallId] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[ActivityId] [uniqueidentifier] NULL,
	[StatusId] [uniqueidentifier] NULL,
	[AssignedUser] [uniqueidentifier] NULL,
	[AssignedTeam] [uniqueidentifier] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_PhoneCalls] PRIMARY KEY CLUSTERED 
(
	[PhoneCallId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Pobability]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pobability](
	[ProbabilityId] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_Portability_ProtabilityId]  DEFAULT (newid()),
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_Pobability] PRIMARY KEY CLUSTERED 
(
	[ProbabilityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Reading]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reading](
	[ReadingId] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_Reading_ReadingId]  DEFAULT (newid()),
	[Value] [decimal](18, 0) NULL,
	[ReadingTypeId] [uniqueidentifier] NULL CONSTRAINT [DF_Reading_ReadingTypeId]  DEFAULT (newid()),
	[ReadingUnitId] [uniqueidentifier] NULL CONSTRAINT [DF_Reading_ReadingUnitId]  DEFAULT (newid()),
	[MarginOfErrorInPercent] [decimal](18, 0) NULL,
	[Description] [nvarchar](500) NULL,
	[IsDeleted] [bit] NULL,
	[StatusId] [uniqueidentifier] NULL,
	[AssignedUser] [uniqueidentifier] NULL,
	[AssignedTeam] [uniqueidentifier] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Reading] PRIMARY KEY CLUSTERED 
(
	[ReadingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ReadingType]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReadingType](
	[ReadingTypeId] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_ReadingType_ReadingTypeId]  DEFAULT (newid()),
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_ReadingType] PRIMARY KEY CLUSTERED 
(
	[ReadingTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ReadingUnit]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReadingUnit](
	[ReadingUnitId] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_ReadingUnit_ReadingUnitId]  DEFAULT (newid()),
	[Name] [nvarchar](50) NULL,
	[Type] [uniqueidentifier] NULL,
 CONSTRAINT [PK_ReadingUnit] PRIMARY KEY CLUSTERED 
(
	[ReadingUnitId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Resource]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Resource](
	[ResourceId] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Address] [uniqueidentifier] NULL,
	[CurrentAddress] [uniqueidentifier] NULL,
	[PhoneHome] [nvarchar](15) NULL,
	[Email] [nvarchar](50) NULL,
	[Website] [nvarchar](50) NULL,
	[PhoneOffice] [nvarchar](15) NULL,
	[Description] [nvarchar](500) NULL,
	[IsDeleted] [bit] NULL,
	[StatusId] [uniqueidentifier] NULL,
	[AssignedUser] [uniqueidentifier] NULL,
	[AssignedTeam] [uniqueidentifier] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Resource] PRIMARY KEY CLUSTERED 
(
	[ResourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ResourceAvailability]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResourceAvailability](
	[ResourceAvailabilityId] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Date] [datetime] NULL,
	[Hour] [decimal](18, 0) NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[Description] [nvarchar](500) NULL,
	[IsDeleted] [bit] NULL,
	[StatusId] [uniqueidentifier] NULL,
	[AssignedUser] [uniqueidentifier] NULL,
	[AssignedTeam] [uniqueidentifier] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_ResourceAvailability] PRIMARY KEY CLUSTERED 
(
	[ResourceAvailabilityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ResourceSkills]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResourceSkills](
	[ResourceSkillId] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[SkillId] [uniqueidentifier] NULL,
	[SkillLevelId] [uniqueidentifier] NULL,
	[Description] [nvarchar](500) NULL,
	[IsDeleted] [bit] NULL,
	[StatusId] [uniqueidentifier] NULL,
	[AssignedUser] [uniqueidentifier] NULL,
	[AssignedTeam] [uniqueidentifier] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_ResourceSkills] PRIMARY KEY CLUSTERED 
(
	[ResourceSkillId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SensorData]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SensorData](
	[SensorDataId] [bigint] IDENTITY(1,1) NOT NULL,
	[DeviceSensorId] [uniqueidentifier] NULL,
	[SensorValue] [float] NULL,
	[RecordDate] [datetime] NULL,
 CONSTRAINT [PK_SensorData] PRIMARY KEY CLUSTERED 
(
	[SensorDataId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ServiceCall]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceCall](
	[ServiceCallId] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_ServiceCall_ServiceCallId]  DEFAULT (newid()),
	[Title] [nvarchar](50) NULL,
	[Detail] [nvarchar](500) NULL,
	[UrgencyId] [uniqueidentifier] NULL CONSTRAINT [DF_ServiceCall_UrgencyId]  DEFAULT (newid()),
	[ServiceCallStageId] [uniqueidentifier] NULL CONSTRAINT [DF_ServiceCall_StageId]  DEFAULT (newid()),
	[Description] [nvarchar](500) NULL,
	[IsDeleted] [bit] NULL,
	[StatusId] [uniqueidentifier] NULL,
	[AssignedUser] [uniqueidentifier] NULL,
	[AssignedTeam] [uniqueidentifier] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_ServiceCall] PRIMARY KEY CLUSTERED 
(
	[ServiceCallId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Skill]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Skill](
	[SkillId] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_Skills_SkillId]  DEFAULT (newid()),
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_Skills] PRIMARY KEY CLUSTERED 
(
	[SkillId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SkillLevel]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SkillLevel](
	[SkillLevelId] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_SkillLevel_SkillLevelId]  DEFAULT (newid()),
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_SkillLevel] PRIMARY KEY CLUSTERED 
(
	[SkillLevelId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Status]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Status](
	[StatusId] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_Status_StatusId]  DEFAULT (newid()),
	[Name] [nvarchar](50) NULL,
	[Type] [nvarchar](50) NULL,
 CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED 
(
	[StatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Task]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Task](
	[TaskId] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[ActivityId] [uniqueidentifier] NULL,
	[StatusId] [uniqueidentifier] NULL,
	[AssignedUser] [uniqueidentifier] NULL,
	[AssignedTeam] [uniqueidentifier] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Task] PRIMARY KEY CLUSTERED 
(
	[TaskId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Team]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Team](
	[TeamId] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_Team_TeamId]  DEFAULT (newid()),
	[Name] [nvarchar](50) NULL,
	[Description] [nvarchar](500) NULL,
	[StatusId] [uniqueidentifier] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Team] PRIMARY KEY CLUSTERED 
(
	[TeamId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TeamUser]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TeamUser](
	[TeamUserId] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_TeamUsers_TeamUserId]  DEFAULT (newid()),
	[UserId] [uniqueidentifier] NULL,
	[TeamId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_TeamUsers] PRIMARY KEY CLUSTERED 
(
	[TeamUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Urgency]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Urgency](
	[UrgencyId] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_Urgency_UrgencyId]  DEFAULT (newid()),
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_Urgency] PRIMARY KEY CLUSTERED 
(
	[UrgencyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[User]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_Users_UserId]  DEFAULT (newid()),
	[Name] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[Phone] [nvarchar](15) NULL,
	[StatusId] [uniqueidentifier] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WorkOrder]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkOrder](
	[WorkOrderId] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Title] [nvarchar](50) NULL,
	[NTE] [decimal](18, 0) NULL,
	[WorkOrderStageId] [uniqueidentifier] NULL,
	[Description] [nvarchar](500) NULL,
	[IsDeleted] [bit] NULL,
	[StatusId] [uniqueidentifier] NULL,
	[AssignedUser] [uniqueidentifier] NULL,
	[AssignedTeam] [uniqueidentifier] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_WorkOrder] PRIMARY KEY CLUSTERED 
(
	[WorkOrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WorkStage]    Script Date: 31/08/2018 4:22:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkStage](
	[WorkStageId] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ServiceCallStages_ServiceCallStagesId]  DEFAULT (newid()),
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_WorkStages] PRIMARY KEY CLUSTERED 
(
	[WorkStageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO
INSERT [dbo].[Account] ([AccountId], [Name], [ShippingAddress], [BillingAddress], [AccountTypeId], [PhoneOffice], [Email], [Fax], [WebSite], [AccountSizeId], [IndustryId], [Description], [IsDeleted], [StatusId], [AssignedUser], [AssignedTeam], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (N'eef12cb5-eba2-4d61-90ab-e48a01f06297', N'Mansoor Siddique', N'e54aefb7-a232-4cf2-9d32-e296a90935ed', N'e54aefb7-a232-4cf2-9d32-e296a90935ed', N'c6f012de-3e7e-41f9-9e3e-30157c627656', N'32132132', N'abuzarva@gmail.com', N'32132132', N'www.techimplement.com', N'e29eb3b9-cabb-4d4e-9509-6c8a65661ebf', N'd13c68db-c797-46ec-93d3-2982bc900fe2', N'test', NULL, N'192f959f-2dfa-4d41-8464-dd482325dc6c', NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[AccountSize] ([AccountSizeId], [Name]) VALUES (N'1b7bfc66-8d96-4b11-865a-39c3b8ce7a42', N'50-200')
GO
INSERT [dbo].[AccountSize] ([AccountSizeId], [Name]) VALUES (N'e29eb3b9-cabb-4d4e-9509-6c8a65661ebf', N'11-50')
GO
INSERT [dbo].[AccountSize] ([AccountSizeId], [Name]) VALUES (N'c41bfb7b-2b0f-4b2e-8199-b6019bf65b77', N'200+')
GO
INSERT [dbo].[AccountSize] ([AccountSizeId], [Name]) VALUES (N'91a2b393-cbd9-4d22-8a5f-ebf5a62c9b53', N'1-10')
GO
INSERT [dbo].[AccountType] ([AccountTypeId], [Name]) VALUES (N'c6f012de-3e7e-41f9-9e3e-30157c627656', N'Vendor')
GO
INSERT [dbo].[AccountType] ([AccountTypeId], [Name]) VALUES (N'21aa76e0-0b31-4836-b5b9-3b5a7718a9db', N'Non-Profit')
GO
INSERT [dbo].[AccountType] ([AccountTypeId], [Name]) VALUES (N'84ce2499-0fab-4883-9aea-6eb923580a0d', N'Company')
GO
INSERT [dbo].[AccountType] ([AccountTypeId], [Name]) VALUES (N'c07fb2f1-5e39-4d92-bfc8-8c75ebdb83b1', N'Competitor')
GO
INSERT [dbo].[AccountType] ([AccountTypeId], [Name]) VALUES (N'5fa5d56f-d2e1-477d-9cc7-c9ee1272f5eb', N'Person')
GO
INSERT [dbo].[Address] ([AddressId], [Street1], [Street2], [City], [State], [Zip], [Country]) VALUES (N'5251f824-a410-4101-80b3-91fe6c377f54', N'Address 1', NULL, N'Lahore', NULL, NULL, N'Pakistan')
GO
INSERT [dbo].[Address] ([AddressId], [Street1], [Street2], [City], [State], [Zip], [Country]) VALUES (N'fe0d6368-50ad-49d1-bc28-a460fef4febd', N'Address 2', NULL, N'Lahore', NULL, NULL, N'Pakistan')
GO
INSERT [dbo].[Address] ([AddressId], [Street1], [Street2], [City], [State], [Zip], [Country]) VALUES (N'e54aefb7-a232-4cf2-9d32-e296a90935ed', N'Gulberg II', N'Forcec Acadmy', N'Lahore', N'Punjab', N'54000', N'Pakistan')
GO
INSERT [dbo].[Address] ([AddressId], [Street1], [Street2], [City], [State], [Zip], [Country]) VALUES (N'f3234ca8-80a2-4b50-8674-fab3c3fb939f', N'Address 3', NULL, N'Lahore', NULL, NULL, N'Pakistan')
GO
INSERT [dbo].[Alert] ([AlertId], [Title], [UrgencyId], [Description], [IsDeleted], [StatusId], [AssignedUser], [AssignedTeam], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (N'f8a63bda-ad9a-4f08-bd1d-66b6e7a7eb3d', N'Alert 1', N'539c1abf-7ddb-4c70-9a07-d7d73da0d038', N'Alert Description 1', NULL, N'192f959f-2dfa-4d41-8464-dd482325dc6c', N'940688bc-da0b-447d-9126-65d91fa4b589', N'92c8677d-5bcb-4ee3-9788-ae1aeb3a8c54', NULL, NULL, N'abuzarva@gmail.com', CAST(N'2018-08-27 15:41:19.367' AS DateTime))
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'Admin', N'Administrator')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'Manager', N'Manager')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'User', N'User')
GO
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'0f213b0a-2b83-4772-8a6a-4c28dbcf31c3', N'Manager')
GO
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'44193866-3f35-42f0-af4d-8969421a7967', N'Admin')
GO
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'0f213b0a-2b83-4772-8a6a-4c28dbcf31c3', N'manager@gmail.com', 0, N'AF5/Ca+tGBpV+WocySbRbTfOom47ikxwfh1OQbK1m42okIe6RljMhAqQ/ROtqjcTgg==', N'172cf487-a5a2-4b9a-bc68-05641c6d6841', NULL, 0, 0, NULL, 1, 0, N'manager@gmail.com')
GO
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'44193866-3f35-42f0-af4d-8969421a7967', N'abuzarva@gmail.com', 0, N'AF7g8gtZyVyDg0XvPBptSb8P8OW9qWrFFaj1O76rbDWY7oRpYChgIq/6TT7uGhyzPQ==', N'a84cbe15-e624-46ac-84e1-1786a277d159', NULL, 0, 0, NULL, 1, 0, N'abuzarva@gmail.com')
GO
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'f4fea153-b94d-4f41-8480-5ec9fb6b645f', N'user1@gmail.com', 0, N'AA2ArgmmU1x/2HC6OFNjvUX+HUZlUaq9n3jgeK5bAXzv40ZMRxpUVJLOJ6vJoJWgmw==', N'527eca70-3fae-4e1b-8a89-6971d5031570', NULL, 0, 0, NULL, 1, 0, N'User1@gmail.com')
GO
INSERT [dbo].[Currency] ([CurrencyId], [Name]) VALUES (N'2210d79a-d2cd-46f9-8ea4-0318e281e9e3', N'AUD')
GO
INSERT [dbo].[Currency] ([CurrencyId], [Name]) VALUES (N'1449af9b-09f9-4214-a95e-0c83cecbe131', N'PKR')
GO
INSERT [dbo].[Currency] ([CurrencyId], [Name]) VALUES (N'43ab94bd-05c4-4577-b25c-4bcc7e208a38', N'USD')
GO
INSERT [dbo].[Currency] ([CurrencyId], [Name]) VALUES (N'50c3698a-316f-4f76-9ac6-858f83013b60', N'EUR')
GO
INSERT [dbo].[CustomerAsset] ([CustomerAssetId], [Title], [CustomerAssetTypeId], [Manufacture], [Model], [YearOfManufacture], [Value], [DepriciatedValue], [SKU], [Description], [IsDeleted], [StatusId], [AssignedUser], [AssignedTeam], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (N'abbb3a02-69bf-4fca-929b-23f18729b6cf', N'Customer Asset', N'9a4336a3-2d6a-4aa3-beb8-13912cfe9ed0', N'Test Manufecturer', N'Test Model', 1983, CAST(500000 AS Decimal(18, 0)), CAST(20000 AS Decimal(18, 0)), N'fasdf-asdfa-sdf-asd-fa-sdf', N'test Description ', NULL, N'192f959f-2dfa-4d41-8464-dd482325dc6c', N'6d60bf41-fb80-4136-a7b9-862e9991256a', N'10c79f67-9fd0-4ddb-b7a2-19a790f44e29', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[CustomerAssetType] ([CustomerAssetTypeId], [Name]) VALUES (N'9a4336a3-2d6a-4aa3-beb8-13912cfe9ed0', N'Projector')
GO
INSERT [dbo].[CustomerAssetType] ([CustomerAssetTypeId], [Name]) VALUES (N'c65a7de5-eeda-4a99-9f92-df14b1fdb824', N'Engine')
GO
INSERT [dbo].[CustomerAssetType] ([CustomerAssetTypeId], [Name]) VALUES (N'e2ac67ce-2fa9-48d6-b2e5-e473b4a5e50f', N'Boiler')
GO
INSERT [dbo].[CustomerAssetType] ([CustomerAssetTypeId], [Name]) VALUES (N'ffbf0003-1866-4513-8be2-e5f76eed5d00', N'Generator')
GO
INSERT [dbo].[Device] ([DeviceId], [Name], [Mac], [EMEINumber], [RegistrationDate], [Latitude], [Longitude], [IsDeleted], [StatusId], [AssignedUser], [AssignedTeam], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (N'b26b503d-a0f4-4ffe-95af-0556d1cddb18', N'Device 1', N'23538353038603jhg', N'Gjvjvnvbv', NULL, CAST(1.00000000 AS Decimal(10, 8)), CAST(1.00000000 AS Decimal(11, 8)), NULL, N'192f959f-2dfa-4d41-8464-dd482325dc6c', N'7a301bfb-c589-443d-924e-fb3a0d1e6a88', N'10c79f67-9fd0-4ddb-b7a2-19a790f44e29', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Industry] ([IndustryId], [Name]) VALUES (N'ba6dd0a3-35d6-4b9b-9d18-051e8881d4da', N'Fitness')
GO
INSERT [dbo].[Industry] ([IndustryId], [Name]) VALUES (N'd13c68db-c797-46ec-93d3-2982bc900fe2', N'Education')
GO
INSERT [dbo].[Industry] ([IndustryId], [Name]) VALUES (N'75a309ac-17c9-4709-8da7-a54f602b75a7', N'Finance')
GO
INSERT [dbo].[Industry] ([IndustryId], [Name]) VALUES (N'83599aed-2bfc-4a98-b6d1-e0b874b6bec0', N'Support')
GO
INSERT [dbo].[Lead] ([LeadId], [Name], [LeadTypeId], [LeadSourceId], [PhoneNumber], [Email], [AddressId], [IndustryId], [Description], [IsDeleted], [StatusId], [AssignedUser], [AssignedTeam], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (N'57beadb5-8e88-4ac5-89b3-78b64eca608f', N'Test Lead ', N'1d096bc5-479c-4dca-aac8-49a6590f8e3d', N'8ac8c3d0-cfbc-4c44-ad73-5a1c544d10b4', N'03344422696', N'manager@gmail.com', N'5251f824-a410-4101-80b3-91fe6c377f54', N'ba6dd0a3-35d6-4b9b-9d18-051e8881d4da', N'This is new Lead Test desciption', NULL, N'192f959f-2dfa-4d41-8464-dd482325dc6c', NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Lead] ([LeadId], [Name], [LeadTypeId], [LeadSourceId], [PhoneNumber], [Email], [AddressId], [IndustryId], [Description], [IsDeleted], [StatusId], [AssignedUser], [AssignedTeam], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (N'f11cb40e-53a2-4a10-88ea-8b9dad2df4c6', N'New Lead 1', N'1d096bc5-479c-4dca-aac8-49a6590f8e3d', N'34547a76-2c62-4bef-ad18-1093d3e8fc01', N'123-123-123', N'abc@xyz.com', N'e54aefb7-a232-4cf2-9d32-e296a90935ed', N'75a309ac-17c9-4709-8da7-a54f602b75a7', N'This is new Lead Two desciption', NULL, N'192f959f-2dfa-4d41-8464-dd482325dc6c', N'6d60bf41-fb80-4136-a7b9-862e9991256a', N'92c8677d-5bcb-4ee3-9788-ae1aeb3a8c54', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Lead] ([LeadId], [Name], [LeadTypeId], [LeadSourceId], [PhoneNumber], [Email], [AddressId], [IndustryId], [Description], [IsDeleted], [StatusId], [AssignedUser], [AssignedTeam], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (N'a65aab03-1d00-440c-80d8-eda834935c5c', N'Mansoor Test Lead', N'1d096bc5-479c-4dca-aac8-49a6590f8e3d', N'34547a76-2c62-4bef-ad18-1093d3e8fc01', N'03344422696', N'abuzarva@gmail.com', N'e54aefb7-a232-4cf2-9d32-e296a90935ed', N'd13c68db-c797-46ec-93d3-2982bc900fe2', N'This is Lead Test Description', NULL, N'192f959f-2dfa-4d41-8464-dd482325dc6c', N'7a301bfb-c589-443d-924e-fb3a0d1e6a88', N'92c8677d-5bcb-4ee3-9788-ae1aeb3a8c54', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[LeadSource] ([LeadSourceId], [Name]) VALUES (N'34547a76-2c62-4bef-ad18-1093d3e8fc01', N'WebSite')
GO
INSERT [dbo].[LeadSource] ([LeadSourceId], [Name]) VALUES (N'8ac8c3d0-cfbc-4c44-ad73-5a1c544d10b4', N'Email')
GO
INSERT [dbo].[LeadSource] ([LeadSourceId], [Name]) VALUES (N'fa5430c2-443e-4f66-8c40-996c94e93eb1', N'Compaign')
GO
INSERT [dbo].[LeadSource] ([LeadSourceId], [Name]) VALUES (N'dd84cabe-bf63-41df-be9b-c8e0a030bf50', N'Confrence')
GO
INSERT [dbo].[LeadType] ([LeadTypeId], [Name]) VALUES (N'1d096bc5-479c-4dca-aac8-49a6590f8e3d', N'Company')
GO
INSERT [dbo].[LeadType] ([LeadTypeId], [Name]) VALUES (N'79191377-eb1b-4b89-9083-9ac408578c56', N'Other')
GO
INSERT [dbo].[LeadType] ([LeadTypeId], [Name]) VALUES (N'231e57c3-414e-4d5f-93e6-d6c4e523c863', N'Person')
GO
INSERT [dbo].[Location] ([LocationId], [Name], [AddressId], [LocationTypeId], [Description], [Latitude], [Longitude], [IsDeleted], [StatusId], [AssignedUser], [AssignedTeam], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (N'822664b9-ac41-46f9-8e39-89ff91b126f0', N'Location 1', N'fe0d6368-50ad-49d1-bc28-a460fef4febd', N'91a2a68a-89fc-4d0a-9ea8-db603cced13e', N'Test Location 1', CAST(1.00000000 AS Decimal(10, 8)), CAST(2.00000000 AS Decimal(11, 8)), NULL, N'192f959f-2dfa-4d41-8464-dd482325dc6c', N'7a301bfb-c589-443d-924e-fb3a0d1e6a88', N'92c8677d-5bcb-4ee3-9788-ae1aeb3a8c54', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[LocationType] ([LocationTypeId], [Name]) VALUES (N'8fe5268c-3893-4e1e-a355-8cd7c554e4ee', N'Branch Office')
GO
INSERT [dbo].[LocationType] ([LocationTypeId], [Name]) VALUES (N'91a2a68a-89fc-4d0a-9ea8-db603cced13e', N'Factory')
GO
INSERT [dbo].[LocationType] ([LocationTypeId], [Name]) VALUES (N'4ec75d27-c213-47a1-80fc-e7a520cee2e3', N'Head Office')
GO
INSERT [dbo].[Opportunity] ([OpportunityId], [Amount], [ProbabilityId], [OpportunityStageId], [Title], [CurrencyId], [Description], [IsDeleted], [StatusId], [AssignedUser], [AssignedTeam], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (N'e9c2a4dd-506b-4cff-9189-1c76e828fc50', CAST(5000 AS Decimal(18, 0)), N'eabd7ca8-2c16-436c-b41e-63cf2079f594', N'dee42c37-a1fb-4ff0-8e32-ceb9aa6d3396', N'Title 1', N'1449af9b-09f9-4214-a95e-0c83cecbe131', N'Description 1', NULL, N'192f959f-2dfa-4d41-8464-dd482325dc6c', N'940688bc-da0b-447d-9126-65d91fa4b589', N'10c79f67-9fd0-4ddb-b7a2-19a790f44e29', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[OpportunityStages] ([OpportunityStageId], [Name]) VALUES (N'e51f45e1-717e-48af-8b2c-2af15f18750e', N'Closed New')
GO
INSERT [dbo].[OpportunityStages] ([OpportunityStageId], [Name]) VALUES (N'1456f7e5-7a93-4d68-9136-ab8154bbd7d4', N'Closed')
GO
INSERT [dbo].[OpportunityStages] ([OpportunityStageId], [Name]) VALUES (N'dee42c37-a1fb-4ff0-8e32-ceb9aa6d3396', N'Negotiation')
GO
INSERT [dbo].[OpportunityStages] ([OpportunityStageId], [Name]) VALUES (N'67efb1d1-a383-47b7-ab5a-d75d4712e0c3', N'Closed Lost')
GO
INSERT [dbo].[Pobability] ([ProbabilityId], [Name]) VALUES (N'46824937-51c5-4a40-8ef8-0a8f8bf79c2c', N'20 %')
GO
INSERT [dbo].[Pobability] ([ProbabilityId], [Name]) VALUES (N'd4c5a083-9328-4e6d-b806-372c92e44dcd', N'60 %')
GO
INSERT [dbo].[Pobability] ([ProbabilityId], [Name]) VALUES (N'8864340b-42f7-4281-83c7-3d896dd68b9a', N'40 %')
GO
INSERT [dbo].[Pobability] ([ProbabilityId], [Name]) VALUES (N'25760c12-a6e0-45af-918d-3eeff0e5afa7', N'10 %')
GO
INSERT [dbo].[Pobability] ([ProbabilityId], [Name]) VALUES (N'd9498e37-3326-430d-9778-5aee369d208f', N'30 %')
GO
INSERT [dbo].[Pobability] ([ProbabilityId], [Name]) VALUES (N'eabd7ca8-2c16-436c-b41e-63cf2079f594', N'90 %')
GO
INSERT [dbo].[Pobability] ([ProbabilityId], [Name]) VALUES (N'f335a5ac-7074-4a1b-a6b5-88beb52a072f', N'50 %')
GO
INSERT [dbo].[Pobability] ([ProbabilityId], [Name]) VALUES (N'84273a75-a125-433f-9c78-b5fbd61d328d', N'80 %')
GO
INSERT [dbo].[Pobability] ([ProbabilityId], [Name]) VALUES (N'86da6527-8162-432a-b23f-b83fb40b0fbb', N'100 %')
GO
INSERT [dbo].[Pobability] ([ProbabilityId], [Name]) VALUES (N'a7b073c0-5981-4761-ba1b-ed02f3b7a55f', N'70 %')
GO
INSERT [dbo].[Reading] ([ReadingId], [Value], [ReadingTypeId], [ReadingUnitId], [MarginOfErrorInPercent], [Description], [IsDeleted], [StatusId], [AssignedUser], [AssignedTeam], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (N'5a56c48a-98f4-4f42-a5d9-fcd4ef5d7fea', CAST(65 AS Decimal(18, 0)), N'40d27797-ba7c-4b89-a997-0f9c9728485b', N'a711912f-08cd-435f-9daf-45715cf82649', CAST(23 AS Decimal(18, 0)), N'test Description ', NULL, N'192f959f-2dfa-4d41-8464-dd482325dc6c', N'6d60bf41-fb80-4136-a7b9-862e9991256a', N'10c79f67-9fd0-4ddb-b7a2-19a790f44e29', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[ReadingType] ([ReadingTypeId], [Name]) VALUES (N'40d27797-ba7c-4b89-a997-0f9c9728485b', N'Temperature')
GO
INSERT [dbo].[ReadingType] ([ReadingTypeId], [Name]) VALUES (N'fc3d80da-4e7b-412f-948a-2fdae7f87288', N'Pressure')
GO
INSERT [dbo].[ReadingUnit] ([ReadingUnitId], [Name], [Type]) VALUES (N'ab36ae59-5379-45ba-a88c-06e7f40041b7', N'Torr', N'fc3d80da-4e7b-412f-948a-2fdae7f87288')
GO
INSERT [dbo].[ReadingUnit] ([ReadingUnitId], [Name], [Type]) VALUES (N'c94868cc-c6e0-4c55-956a-19d4df74c3d0', N'ATM', N'fc3d80da-4e7b-412f-948a-2fdae7f87288')
GO
INSERT [dbo].[ReadingUnit] ([ReadingUnitId], [Name], [Type]) VALUES (N'38e4d471-2d0a-47e0-8961-3c1b62bdda11', N'Kelvin', N'40d27797-ba7c-4b89-a997-0f9c9728485b')
GO
INSERT [dbo].[ReadingUnit] ([ReadingUnitId], [Name], [Type]) VALUES (N'b87f0d2c-259e-4a16-8313-3c739316ed89', N'PSI', N'fc3d80da-4e7b-412f-948a-2fdae7f87288')
GO
INSERT [dbo].[ReadingUnit] ([ReadingUnitId], [Name], [Type]) VALUES (N'a711912f-08cd-435f-9daf-45715cf82649', N'Fahrenheit', N'40d27797-ba7c-4b89-a997-0f9c9728485b')
GO
INSERT [dbo].[ReadingUnit] ([ReadingUnitId], [Name], [Type]) VALUES (N'd50851d3-7ede-4695-9e9c-6f447d6172d6', N'Celsius', N'40d27797-ba7c-4b89-a997-0f9c9728485b')
GO
INSERT [dbo].[ReadingUnit] ([ReadingUnitId], [Name], [Type]) VALUES (N'ade64091-1673-4875-8d97-8ec79eadd4b6', N'Bar', N'fc3d80da-4e7b-412f-948a-2fdae7f87288')
GO
INSERT [dbo].[ReadingUnit] ([ReadingUnitId], [Name], [Type]) VALUES (N'3c6c26d4-0fae-4130-93d2-a6a707324b11', N'mmhg', N'fc3d80da-4e7b-412f-948a-2fdae7f87288')
GO
INSERT [dbo].[ReadingUnit] ([ReadingUnitId], [Name], [Type]) VALUES (N'6af79604-9331-4ebd-a5d9-bfa72bef96d9', N'Pascal', N'fc3d80da-4e7b-412f-948a-2fdae7f87288')
GO
INSERT [dbo].[ServiceCall] ([ServiceCallId], [Title], [Detail], [UrgencyId], [ServiceCallStageId], [Description], [IsDeleted], [StatusId], [AssignedUser], [AssignedTeam], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (N'23b6acbe-e028-47bd-ae39-a6d8b7bc72b5', N'Service Call 1', N'Service Call Detail', N'8fd3c9e4-e371-4816-9994-10f3a4376582', N'8c2afafd-ddf9-46d7-83ee-d46eadef17d9', N'Service Call  Description', NULL, N'192f959f-2dfa-4d41-8464-dd482325dc6c', N'940688bc-da0b-447d-9126-65d91fa4b589', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Skill] ([SkillId], [Name]) VALUES (N'02c551fe-945f-49cd-abdb-079f8ca31ee1', N'Piping')
GO
INSERT [dbo].[Skill] ([SkillId], [Name]) VALUES (N'2e4fdb6a-1048-4070-bd84-76ddf0071721', N'Plumbing')
GO
INSERT [dbo].[Skill] ([SkillId], [Name]) VALUES (N'97bf4486-6df9-4aef-bc57-77aa047f0cd7', N'Cleaning')
GO
INSERT [dbo].[Skill] ([SkillId], [Name]) VALUES (N'72abad65-f561-4b21-93db-996c9c7707f0', N'Electrical')
GO
INSERT [dbo].[SkillLevel] ([SkillLevelId], [Name]) VALUES (N'0a641e2a-5cdc-472f-a079-13aa7d11e801', N'2')
GO
INSERT [dbo].[SkillLevel] ([SkillLevelId], [Name]) VALUES (N'b4d1fb82-a7e6-40d5-9e12-7831226906fd', N'5')
GO
INSERT [dbo].[SkillLevel] ([SkillLevelId], [Name]) VALUES (N'ee05e6b3-3119-4f9f-be73-974d3493109f', N'3')
GO
INSERT [dbo].[SkillLevel] ([SkillLevelId], [Name]) VALUES (N'051057b2-b1e2-4aef-937c-edb6e4d2d0f6', N'4')
GO
INSERT [dbo].[SkillLevel] ([SkillLevelId], [Name]) VALUES (N'185600b8-760f-45c9-8e4d-f1dc49a4b364', N'1')
GO
INSERT [dbo].[Status] ([StatusId], [Name], [Type]) VALUES (N'fb6bab54-3e26-4270-a875-34bc7f72afd8', N'InActive', NULL)
GO
INSERT [dbo].[Status] ([StatusId], [Name], [Type]) VALUES (N'192f959f-2dfa-4d41-8464-dd482325dc6c', N'Active', NULL)
GO
INSERT [dbo].[Team] ([TeamId], [Name], [Description], [StatusId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (N'10c79f67-9fd0-4ddb-b7a2-19a790f44e29', N'Team A', N'A Category Team', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Team] ([TeamId], [Name], [Description], [StatusId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (N'92c8677d-5bcb-4ee3-9788-ae1aeb3a8c54', N'Team B', N'B Category Team', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[TeamUser] ([TeamUserId], [UserId], [TeamId]) VALUES (N'2bb21725-836d-4088-adfb-09643eb9611b', N'7a301bfb-c589-443d-924e-fb3a0d1e6a88', N'10c79f67-9fd0-4ddb-b7a2-19a790f44e29')
GO
INSERT [dbo].[TeamUser] ([TeamUserId], [UserId], [TeamId]) VALUES (N'35fad3ef-1b18-4f92-8486-6079260c29fb', N'940688bc-da0b-447d-9126-65d91fa4b589', N'10c79f67-9fd0-4ddb-b7a2-19a790f44e29')
GO
INSERT [dbo].[Urgency] ([UrgencyId], [Name]) VALUES (N'8fd3c9e4-e371-4816-9994-10f3a4376582', N'Medium')
GO
INSERT [dbo].[Urgency] ([UrgencyId], [Name]) VALUES (N'4e695b09-9abd-45c0-98a5-a907de1663b8', N'High')
GO
INSERT [dbo].[Urgency] ([UrgencyId], [Name]) VALUES (N'539c1abf-7ddb-4c70-9a07-d7d73da0d038', N'Low')
GO
INSERT [dbo].[User] ([UserId], [Name], [Email], [Phone], [StatusId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (N'940688bc-da0b-447d-9126-65d91fa4b589', N'User 2', N'user2@xyz.com', N'0300-2323232', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[User] ([UserId], [Name], [Email], [Phone], [StatusId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (N'6d60bf41-fb80-4136-a7b9-862e9991256a', N'User 3', N'user3@abc.com', N'0345-3213212', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[User] ([UserId], [Name], [Email], [Phone], [StatusId], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate]) VALUES (N'7a301bfb-c589-443d-924e-fb3a0d1e6a88', N'User 1', N'user1@abc.com', N'0333-1234567', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[WorkStage] ([WorkStageId], [Name]) VALUES (N'ad589767-33b6-455b-893e-19c0af341879', N'Canceled')
GO
INSERT [dbo].[WorkStage] ([WorkStageId], [Name]) VALUES (N'5fae2aa8-aca0-4c77-8f6d-1aab9244f4e5', N'Completed')
GO
INSERT [dbo].[WorkStage] ([WorkStageId], [Name]) VALUES (N'1e8772a3-07b8-42f7-90fa-b4b3f82dc38a', N'Under Progress')
GO
INSERT [dbo].[WorkStage] ([WorkStageId], [Name]) VALUES (N'8c2afafd-ddf9-46d7-83ee-d46eadef17d9', N'Resolved')
GO
INSERT [dbo].[WorkStage] ([WorkStageId], [Name]) VALUES (N'fd9313c9-65c9-44b7-b34f-f67fd7a8f90c', N'New')
GO
ALTER TABLE [dbo].[Activity] ADD  CONSTRAINT [DF_Activity_ActivityId]  DEFAULT (newid()) FOR [ActivityId]
GO
ALTER TABLE [dbo].[Category] ADD  CONSTRAINT [DF_Category_CategoryId]  DEFAULT (newid()) FOR [CategoryId]
GO
ALTER TABLE [dbo].[DeciveConfiguration] ADD  CONSTRAINT [DF_DeciveConfiguration_DeviceConfigurationId]  DEFAULT (newid()) FOR [DeviceConfigurationId]
GO
ALTER TABLE [dbo].[DeciveConfiguration] ADD  CONSTRAINT [DF_DeciveConfiguration_DeviceId]  DEFAULT (newid()) FOR [DeviceId]
GO
ALTER TABLE [dbo].[DeviceSensor] ADD  CONSTRAINT [DF_DeviceSensor_DeviceSensorId]  DEFAULT (newid()) FOR [DeviceSensorId]
GO
ALTER TABLE [dbo].[Email] ADD  CONSTRAINT [DF_Email_EmailId]  DEFAULT (newid()) FOR [EmailId]
GO
ALTER TABLE [dbo].[Email] ADD  CONSTRAINT [DF_Email_ActivityId]  DEFAULT (newid()) FOR [ActivityId]
GO
ALTER TABLE [dbo].[Meeting] ADD  CONSTRAINT [DF_Meeting_MeetingId]  DEFAULT (newid()) FOR [MeetingId]
GO
ALTER TABLE [dbo].[Meeting] ADD  CONSTRAINT [DF_Meeting_ActivityId]  DEFAULT (newid()) FOR [ActivityId]
GO
ALTER TABLE [dbo].[PhoneCall] ADD  CONSTRAINT [DF_PhoneCalls_PhoneCallId]  DEFAULT (newid()) FOR [PhoneCallId]
GO
ALTER TABLE [dbo].[PhoneCall] ADD  CONSTRAINT [DF_PhoneCalls_ActivityId]  DEFAULT (newid()) FOR [ActivityId]
GO
ALTER TABLE [dbo].[Resource] ADD  CONSTRAINT [DF_Resource_ResourceId]  DEFAULT (newid()) FOR [ResourceId]
GO
ALTER TABLE [dbo].[Resource] ADD  CONSTRAINT [DF_Resource_AddressId]  DEFAULT (newid()) FOR [Address]
GO
ALTER TABLE [dbo].[ResourceAvailability] ADD  CONSTRAINT [DF_ResourceAvailability_ResourceAvailabilityId]  DEFAULT (newid()) FOR [ResourceAvailabilityId]
GO
ALTER TABLE [dbo].[ResourceSkills] ADD  CONSTRAINT [DF_ResourceSkills_ResourceSkillId]  DEFAULT (newid()) FOR [ResourceSkillId]
GO
ALTER TABLE [dbo].[ResourceSkills] ADD  CONSTRAINT [DF_ResourceSkills_SkillId]  DEFAULT (newid()) FOR [SkillId]
GO
ALTER TABLE [dbo].[ResourceSkills] ADD  CONSTRAINT [DF_ResourceSkills_SkillLevelId]  DEFAULT (newid()) FOR [SkillLevelId]
GO
ALTER TABLE [dbo].[SensorData] ADD  CONSTRAINT [DF_SensorData_DeviceSensorId]  DEFAULT (newid()) FOR [DeviceSensorId]
GO
ALTER TABLE [dbo].[Task] ADD  CONSTRAINT [DF_Task_TaskId]  DEFAULT (newid()) FOR [TaskId]
GO
ALTER TABLE [dbo].[Task] ADD  CONSTRAINT [DF_Task_ActivityId]  DEFAULT (newid()) FOR [ActivityId]
GO
ALTER TABLE [dbo].[WorkOrder] ADD  CONSTRAINT [DF_WorkOrder_WorkOrderId]  DEFAULT (newid()) FOR [WorkOrderId]
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [FK_Account_AccountSize] FOREIGN KEY([AccountSizeId])
REFERENCES [dbo].[AccountSize] ([AccountSizeId])
GO
ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [FK_Account_AccountSize]
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [FK_Account_AccountType] FOREIGN KEY([AccountTypeId])
REFERENCES [dbo].[AccountType] ([AccountTypeId])
GO
ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [FK_Account_AccountType]
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [FK_Account_Address] FOREIGN KEY([ShippingAddress])
REFERENCES [dbo].[Address] ([AddressId])
GO
ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [FK_Account_Address]
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [FK_Account_Address1] FOREIGN KEY([BillingAddress])
REFERENCES [dbo].[Address] ([AddressId])
GO
ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [FK_Account_Address1]
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [FK_Account_Industry] FOREIGN KEY([IndustryId])
REFERENCES [dbo].[Industry] ([IndustryId])
GO
ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [FK_Account_Industry]
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [FK_Account_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [FK_Account_Status]
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [FK_Account_Team] FOREIGN KEY([AssignedTeam])
REFERENCES [dbo].[Team] ([TeamId])
GO
ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [FK_Account_Team]
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [FK_Account_Users] FOREIGN KEY([AssignedUser])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [FK_Account_Users]
GO
ALTER TABLE [dbo].[Activity]  WITH CHECK ADD  CONSTRAINT [FK_Activity_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[Activity] CHECK CONSTRAINT [FK_Activity_Status]
GO
ALTER TABLE [dbo].[Activity]  WITH CHECK ADD  CONSTRAINT [FK_Activity_Team] FOREIGN KEY([AssignedTeam])
REFERENCES [dbo].[Team] ([TeamId])
GO
ALTER TABLE [dbo].[Activity] CHECK CONSTRAINT [FK_Activity_Team]
GO
ALTER TABLE [dbo].[Activity]  WITH CHECK ADD  CONSTRAINT [FK_Activity_User] FOREIGN KEY([AssignedUser])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Activity] CHECK CONSTRAINT [FK_Activity_User]
GO
ALTER TABLE [dbo].[Alert]  WITH CHECK ADD  CONSTRAINT [FK_Alert_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[Alert] CHECK CONSTRAINT [FK_Alert_Status]
GO
ALTER TABLE [dbo].[Alert]  WITH CHECK ADD  CONSTRAINT [FK_Alert_Team] FOREIGN KEY([AssignedTeam])
REFERENCES [dbo].[Team] ([TeamId])
GO
ALTER TABLE [dbo].[Alert] CHECK CONSTRAINT [FK_Alert_Team]
GO
ALTER TABLE [dbo].[Alert]  WITH CHECK ADD  CONSTRAINT [FK_Alert_Urgency] FOREIGN KEY([UrgencyId])
REFERENCES [dbo].[Urgency] ([UrgencyId])
GO
ALTER TABLE [dbo].[Alert] CHECK CONSTRAINT [FK_Alert_Urgency]
GO
ALTER TABLE [dbo].[Alert]  WITH CHECK ADD  CONSTRAINT [FK_Alert_Users] FOREIGN KEY([AssignedUser])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Alert] CHECK CONSTRAINT [FK_Alert_Users]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Category]  WITH CHECK ADD  CONSTRAINT [FK_Category_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[Category] CHECK CONSTRAINT [FK_Category_Status]
GO
ALTER TABLE [dbo].[Category]  WITH CHECK ADD  CONSTRAINT [FK_Category_Team] FOREIGN KEY([AssignedTeam])
REFERENCES [dbo].[Team] ([TeamId])
GO
ALTER TABLE [dbo].[Category] CHECK CONSTRAINT [FK_Category_Team]
GO
ALTER TABLE [dbo].[Category]  WITH CHECK ADD  CONSTRAINT [FK_Category_Users] FOREIGN KEY([AssignedUser])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Category] CHECK CONSTRAINT [FK_Category_Users]
GO
ALTER TABLE [dbo].[CustomerAsset]  WITH CHECK ADD  CONSTRAINT [FK_CustomerAsset_CustomerAssetType] FOREIGN KEY([CustomerAssetTypeId])
REFERENCES [dbo].[CustomerAssetType] ([CustomerAssetTypeId])
GO
ALTER TABLE [dbo].[CustomerAsset] CHECK CONSTRAINT [FK_CustomerAsset_CustomerAssetType]
GO
ALTER TABLE [dbo].[CustomerAsset]  WITH CHECK ADD  CONSTRAINT [FK_CustomerAsset_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[CustomerAsset] CHECK CONSTRAINT [FK_CustomerAsset_Status]
GO
ALTER TABLE [dbo].[CustomerAsset]  WITH CHECK ADD  CONSTRAINT [FK_CustomerAsset_Team] FOREIGN KEY([AssignedTeam])
REFERENCES [dbo].[Team] ([TeamId])
GO
ALTER TABLE [dbo].[CustomerAsset] CHECK CONSTRAINT [FK_CustomerAsset_Team]
GO
ALTER TABLE [dbo].[CustomerAsset]  WITH CHECK ADD  CONSTRAINT [FK_CustomerAsset_Users] FOREIGN KEY([AssignedUser])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[CustomerAsset] CHECK CONSTRAINT [FK_CustomerAsset_Users]
GO
ALTER TABLE [dbo].[DeciveConfiguration]  WITH CHECK ADD  CONSTRAINT [FK_DeciveConfiguration_Device] FOREIGN KEY([DeviceId])
REFERENCES [dbo].[Device] ([DeviceId])
GO
ALTER TABLE [dbo].[DeciveConfiguration] CHECK CONSTRAINT [FK_DeciveConfiguration_Device]
GO
ALTER TABLE [dbo].[Device]  WITH CHECK ADD  CONSTRAINT [FK_Devices_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[Device] CHECK CONSTRAINT [FK_Devices_Status]
GO
ALTER TABLE [dbo].[Device]  WITH CHECK ADD  CONSTRAINT [FK_Devices_Team] FOREIGN KEY([AssignedTeam])
REFERENCES [dbo].[Team] ([TeamId])
GO
ALTER TABLE [dbo].[Device] CHECK CONSTRAINT [FK_Devices_Team]
GO
ALTER TABLE [dbo].[Device]  WITH CHECK ADD  CONSTRAINT [FK_Devices_User] FOREIGN KEY([AssignedUser])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Device] CHECK CONSTRAINT [FK_Devices_User]
GO
ALTER TABLE [dbo].[DeviceSensor]  WITH CHECK ADD  CONSTRAINT [FK_DeviceSensor_Device] FOREIGN KEY([DeviceId])
REFERENCES [dbo].[Device] ([DeviceId])
GO
ALTER TABLE [dbo].[DeviceSensor] CHECK CONSTRAINT [FK_DeviceSensor_Device]
GO
ALTER TABLE [dbo].[DeviceSensor]  WITH CHECK ADD  CONSTRAINT [FK_DeviceSensor_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[DeviceSensor] CHECK CONSTRAINT [FK_DeviceSensor_Status]
GO
ALTER TABLE [dbo].[Email]  WITH CHECK ADD  CONSTRAINT [FK_Email_Activity] FOREIGN KEY([ActivityId])
REFERENCES [dbo].[Activity] ([ActivityId])
GO
ALTER TABLE [dbo].[Email] CHECK CONSTRAINT [FK_Email_Activity]
GO
ALTER TABLE [dbo].[Email]  WITH CHECK ADD  CONSTRAINT [FK_Email_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[Email] CHECK CONSTRAINT [FK_Email_Status]
GO
ALTER TABLE [dbo].[Email]  WITH CHECK ADD  CONSTRAINT [FK_Email_Team] FOREIGN KEY([AssignedTeam])
REFERENCES [dbo].[Team] ([TeamId])
GO
ALTER TABLE [dbo].[Email] CHECK CONSTRAINT [FK_Email_Team]
GO
ALTER TABLE [dbo].[Email]  WITH CHECK ADD  CONSTRAINT [FK_Email_User] FOREIGN KEY([AssignedUser])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Email] CHECK CONSTRAINT [FK_Email_User]
GO
ALTER TABLE [dbo].[Lead]  WITH CHECK ADD  CONSTRAINT [FK_Lead_Address] FOREIGN KEY([AddressId])
REFERENCES [dbo].[Address] ([AddressId])
GO
ALTER TABLE [dbo].[Lead] CHECK CONSTRAINT [FK_Lead_Address]
GO
ALTER TABLE [dbo].[Lead]  WITH CHECK ADD  CONSTRAINT [FK_Lead_Industry] FOREIGN KEY([IndustryId])
REFERENCES [dbo].[Industry] ([IndustryId])
GO
ALTER TABLE [dbo].[Lead] CHECK CONSTRAINT [FK_Lead_Industry]
GO
ALTER TABLE [dbo].[Lead]  WITH CHECK ADD  CONSTRAINT [FK_Lead_LeadSource] FOREIGN KEY([LeadSourceId])
REFERENCES [dbo].[LeadSource] ([LeadSourceId])
GO
ALTER TABLE [dbo].[Lead] CHECK CONSTRAINT [FK_Lead_LeadSource]
GO
ALTER TABLE [dbo].[Lead]  WITH CHECK ADD  CONSTRAINT [FK_Lead_LeadType] FOREIGN KEY([LeadTypeId])
REFERENCES [dbo].[LeadType] ([LeadTypeId])
GO
ALTER TABLE [dbo].[Lead] CHECK CONSTRAINT [FK_Lead_LeadType]
GO
ALTER TABLE [dbo].[Lead]  WITH CHECK ADD  CONSTRAINT [FK_Lead_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[Lead] CHECK CONSTRAINT [FK_Lead_Status]
GO
ALTER TABLE [dbo].[Lead]  WITH CHECK ADD  CONSTRAINT [FK_Lead_Team] FOREIGN KEY([AssignedTeam])
REFERENCES [dbo].[Team] ([TeamId])
GO
ALTER TABLE [dbo].[Lead] CHECK CONSTRAINT [FK_Lead_Team]
GO
ALTER TABLE [dbo].[Lead]  WITH CHECK ADD  CONSTRAINT [FK_Lead_Users] FOREIGN KEY([AssignedUser])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Lead] CHECK CONSTRAINT [FK_Lead_Users]
GO
ALTER TABLE [dbo].[Location]  WITH CHECK ADD  CONSTRAINT [FK_Location_Address] FOREIGN KEY([AddressId])
REFERENCES [dbo].[Address] ([AddressId])
GO
ALTER TABLE [dbo].[Location] CHECK CONSTRAINT [FK_Location_Address]
GO
ALTER TABLE [dbo].[Location]  WITH CHECK ADD  CONSTRAINT [FK_Location_LocationType] FOREIGN KEY([LocationTypeId])
REFERENCES [dbo].[LocationType] ([LocationTypeId])
GO
ALTER TABLE [dbo].[Location] CHECK CONSTRAINT [FK_Location_LocationType]
GO
ALTER TABLE [dbo].[Location]  WITH CHECK ADD  CONSTRAINT [FK_Location_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[Location] CHECK CONSTRAINT [FK_Location_Status]
GO
ALTER TABLE [dbo].[Location]  WITH CHECK ADD  CONSTRAINT [FK_Location_Team] FOREIGN KEY([AssignedTeam])
REFERENCES [dbo].[Team] ([TeamId])
GO
ALTER TABLE [dbo].[Location] CHECK CONSTRAINT [FK_Location_Team]
GO
ALTER TABLE [dbo].[Location]  WITH CHECK ADD  CONSTRAINT [FK_Location_Users] FOREIGN KEY([AssignedUser])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Location] CHECK CONSTRAINT [FK_Location_Users]
GO
ALTER TABLE [dbo].[Meeting]  WITH CHECK ADD  CONSTRAINT [FK_Meeting_Activity] FOREIGN KEY([ActivityId])
REFERENCES [dbo].[Activity] ([ActivityId])
GO
ALTER TABLE [dbo].[Meeting] CHECK CONSTRAINT [FK_Meeting_Activity]
GO
ALTER TABLE [dbo].[Meeting]  WITH CHECK ADD  CONSTRAINT [FK_Meeting_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[Meeting] CHECK CONSTRAINT [FK_Meeting_Status]
GO
ALTER TABLE [dbo].[Meeting]  WITH CHECK ADD  CONSTRAINT [FK_Meeting_Team] FOREIGN KEY([AssignedTeam])
REFERENCES [dbo].[Team] ([TeamId])
GO
ALTER TABLE [dbo].[Meeting] CHECK CONSTRAINT [FK_Meeting_Team]
GO
ALTER TABLE [dbo].[Meeting]  WITH CHECK ADD  CONSTRAINT [FK_Meeting_User] FOREIGN KEY([AssignedUser])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Meeting] CHECK CONSTRAINT [FK_Meeting_User]
GO
ALTER TABLE [dbo].[Opportunity]  WITH CHECK ADD  CONSTRAINT [FK_Oppertunity_Currency] FOREIGN KEY([CurrencyId])
REFERENCES [dbo].[Currency] ([CurrencyId])
GO
ALTER TABLE [dbo].[Opportunity] CHECK CONSTRAINT [FK_Oppertunity_Currency]
GO
ALTER TABLE [dbo].[Opportunity]  WITH CHECK ADD  CONSTRAINT [FK_Oppertunity_Team] FOREIGN KEY([AssignedTeam])
REFERENCES [dbo].[Team] ([TeamId])
GO
ALTER TABLE [dbo].[Opportunity] CHECK CONSTRAINT [FK_Oppertunity_Team]
GO
ALTER TABLE [dbo].[Opportunity]  WITH CHECK ADD  CONSTRAINT [FK_Oppertunity_Users] FOREIGN KEY([AssignedUser])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Opportunity] CHECK CONSTRAINT [FK_Oppertunity_Users]
GO
ALTER TABLE [dbo].[Opportunity]  WITH CHECK ADD  CONSTRAINT [FK_Opportunity_OpportunityStages] FOREIGN KEY([OpportunityStageId])
REFERENCES [dbo].[OpportunityStages] ([OpportunityStageId])
GO
ALTER TABLE [dbo].[Opportunity] CHECK CONSTRAINT [FK_Opportunity_OpportunityStages]
GO
ALTER TABLE [dbo].[Opportunity]  WITH CHECK ADD  CONSTRAINT [FK_Opportunity_Pobability] FOREIGN KEY([ProbabilityId])
REFERENCES [dbo].[Pobability] ([ProbabilityId])
GO
ALTER TABLE [dbo].[Opportunity] CHECK CONSTRAINT [FK_Opportunity_Pobability]
GO
ALTER TABLE [dbo].[Opportunity]  WITH CHECK ADD  CONSTRAINT [FK_Opportunity_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[Opportunity] CHECK CONSTRAINT [FK_Opportunity_Status]
GO
ALTER TABLE [dbo].[PhoneCall]  WITH CHECK ADD  CONSTRAINT [FK_PhoneCall_Activity] FOREIGN KEY([ActivityId])
REFERENCES [dbo].[Activity] ([ActivityId])
GO
ALTER TABLE [dbo].[PhoneCall] CHECK CONSTRAINT [FK_PhoneCall_Activity]
GO
ALTER TABLE [dbo].[PhoneCall]  WITH CHECK ADD  CONSTRAINT [FK_PhoneCall_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[PhoneCall] CHECK CONSTRAINT [FK_PhoneCall_Status]
GO
ALTER TABLE [dbo].[PhoneCall]  WITH CHECK ADD  CONSTRAINT [FK_PhoneCall_Team] FOREIGN KEY([AssignedTeam])
REFERENCES [dbo].[Team] ([TeamId])
GO
ALTER TABLE [dbo].[PhoneCall] CHECK CONSTRAINT [FK_PhoneCall_Team]
GO
ALTER TABLE [dbo].[PhoneCall]  WITH CHECK ADD  CONSTRAINT [FK_PhoneCall_User] FOREIGN KEY([AssignedUser])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[PhoneCall] CHECK CONSTRAINT [FK_PhoneCall_User]
GO
ALTER TABLE [dbo].[Reading]  WITH CHECK ADD  CONSTRAINT [FK_Reading_ReadingType] FOREIGN KEY([ReadingTypeId])
REFERENCES [dbo].[ReadingType] ([ReadingTypeId])
GO
ALTER TABLE [dbo].[Reading] CHECK CONSTRAINT [FK_Reading_ReadingType]
GO
ALTER TABLE [dbo].[Reading]  WITH CHECK ADD  CONSTRAINT [FK_Reading_ReadingUnit] FOREIGN KEY([ReadingUnitId])
REFERENCES [dbo].[ReadingUnit] ([ReadingUnitId])
GO
ALTER TABLE [dbo].[Reading] CHECK CONSTRAINT [FK_Reading_ReadingUnit]
GO
ALTER TABLE [dbo].[Reading]  WITH CHECK ADD  CONSTRAINT [FK_Reading_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[Reading] CHECK CONSTRAINT [FK_Reading_Status]
GO
ALTER TABLE [dbo].[Reading]  WITH CHECK ADD  CONSTRAINT [FK_Reading_Team] FOREIGN KEY([AssignedTeam])
REFERENCES [dbo].[Team] ([TeamId])
GO
ALTER TABLE [dbo].[Reading] CHECK CONSTRAINT [FK_Reading_Team]
GO
ALTER TABLE [dbo].[Reading]  WITH CHECK ADD  CONSTRAINT [FK_Reading_Users] FOREIGN KEY([AssignedUser])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Reading] CHECK CONSTRAINT [FK_Reading_Users]
GO
ALTER TABLE [dbo].[ReadingUnit]  WITH CHECK ADD  CONSTRAINT [FK_ReadingUnit_ReadingType] FOREIGN KEY([Type])
REFERENCES [dbo].[ReadingType] ([ReadingTypeId])
GO
ALTER TABLE [dbo].[ReadingUnit] CHECK CONSTRAINT [FK_ReadingUnit_ReadingType]
GO
ALTER TABLE [dbo].[Resource]  WITH CHECK ADD  CONSTRAINT [FK_Resource_Address] FOREIGN KEY([Address])
REFERENCES [dbo].[Address] ([AddressId])
GO
ALTER TABLE [dbo].[Resource] CHECK CONSTRAINT [FK_Resource_Address]
GO
ALTER TABLE [dbo].[Resource]  WITH CHECK ADD  CONSTRAINT [FK_Resource_Address1] FOREIGN KEY([CurrentAddress])
REFERENCES [dbo].[Address] ([AddressId])
GO
ALTER TABLE [dbo].[Resource] CHECK CONSTRAINT [FK_Resource_Address1]
GO
ALTER TABLE [dbo].[Resource]  WITH CHECK ADD  CONSTRAINT [FK_Resource_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[Resource] CHECK CONSTRAINT [FK_Resource_Status]
GO
ALTER TABLE [dbo].[Resource]  WITH CHECK ADD  CONSTRAINT [FK_Resource_Team] FOREIGN KEY([AssignedTeam])
REFERENCES [dbo].[Team] ([TeamId])
GO
ALTER TABLE [dbo].[Resource] CHECK CONSTRAINT [FK_Resource_Team]
GO
ALTER TABLE [dbo].[Resource]  WITH CHECK ADD  CONSTRAINT [FK_Resource_Users] FOREIGN KEY([AssignedUser])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Resource] CHECK CONSTRAINT [FK_Resource_Users]
GO
ALTER TABLE [dbo].[ResourceAvailability]  WITH CHECK ADD  CONSTRAINT [FK_ResourceAvailability_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[ResourceAvailability] CHECK CONSTRAINT [FK_ResourceAvailability_Status]
GO
ALTER TABLE [dbo].[ResourceAvailability]  WITH CHECK ADD  CONSTRAINT [FK_ResourceAvailability_Team] FOREIGN KEY([AssignedTeam])
REFERENCES [dbo].[Team] ([TeamId])
GO
ALTER TABLE [dbo].[ResourceAvailability] CHECK CONSTRAINT [FK_ResourceAvailability_Team]
GO
ALTER TABLE [dbo].[ResourceAvailability]  WITH CHECK ADD  CONSTRAINT [FK_ResourceAvailability_Users] FOREIGN KEY([AssignedUser])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[ResourceAvailability] CHECK CONSTRAINT [FK_ResourceAvailability_Users]
GO
ALTER TABLE [dbo].[ResourceSkills]  WITH CHECK ADD  CONSTRAINT [FK_ResourceSkills_SkillLevel] FOREIGN KEY([SkillLevelId])
REFERENCES [dbo].[SkillLevel] ([SkillLevelId])
GO
ALTER TABLE [dbo].[ResourceSkills] CHECK CONSTRAINT [FK_ResourceSkills_SkillLevel]
GO
ALTER TABLE [dbo].[ResourceSkills]  WITH CHECK ADD  CONSTRAINT [FK_ResourceSkills_Skills] FOREIGN KEY([SkillId])
REFERENCES [dbo].[Skill] ([SkillId])
GO
ALTER TABLE [dbo].[ResourceSkills] CHECK CONSTRAINT [FK_ResourceSkills_Skills]
GO
ALTER TABLE [dbo].[ResourceSkills]  WITH CHECK ADD  CONSTRAINT [FK_ResourceSkills_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[ResourceSkills] CHECK CONSTRAINT [FK_ResourceSkills_Status]
GO
ALTER TABLE [dbo].[ResourceSkills]  WITH CHECK ADD  CONSTRAINT [FK_ResourceSkills_Team] FOREIGN KEY([AssignedTeam])
REFERENCES [dbo].[Team] ([TeamId])
GO
ALTER TABLE [dbo].[ResourceSkills] CHECK CONSTRAINT [FK_ResourceSkills_Team]
GO
ALTER TABLE [dbo].[ResourceSkills]  WITH CHECK ADD  CONSTRAINT [FK_ResourceSkills_Users] FOREIGN KEY([AssignedUser])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[ResourceSkills] CHECK CONSTRAINT [FK_ResourceSkills_Users]
GO
ALTER TABLE [dbo].[SensorData]  WITH CHECK ADD  CONSTRAINT [FK_SensorData_DeviceSensor] FOREIGN KEY([DeviceSensorId])
REFERENCES [dbo].[DeviceSensor] ([DeviceSensorId])
GO
ALTER TABLE [dbo].[SensorData] CHECK CONSTRAINT [FK_SensorData_DeviceSensor]
GO
ALTER TABLE [dbo].[ServiceCall]  WITH CHECK ADD  CONSTRAINT [FK_ServiceCall_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[ServiceCall] CHECK CONSTRAINT [FK_ServiceCall_Status]
GO
ALTER TABLE [dbo].[ServiceCall]  WITH CHECK ADD  CONSTRAINT [FK_ServiceCall_Team] FOREIGN KEY([AssignedTeam])
REFERENCES [dbo].[Team] ([TeamId])
GO
ALTER TABLE [dbo].[ServiceCall] CHECK CONSTRAINT [FK_ServiceCall_Team]
GO
ALTER TABLE [dbo].[ServiceCall]  WITH CHECK ADD  CONSTRAINT [FK_ServiceCall_Urgency] FOREIGN KEY([UrgencyId])
REFERENCES [dbo].[Urgency] ([UrgencyId])
GO
ALTER TABLE [dbo].[ServiceCall] CHECK CONSTRAINT [FK_ServiceCall_Urgency]
GO
ALTER TABLE [dbo].[ServiceCall]  WITH CHECK ADD  CONSTRAINT [FK_ServiceCall_Users] FOREIGN KEY([AssignedUser])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[ServiceCall] CHECK CONSTRAINT [FK_ServiceCall_Users]
GO
ALTER TABLE [dbo].[ServiceCall]  WITH CHECK ADD  CONSTRAINT [FK_ServiceCall_WorkStages] FOREIGN KEY([ServiceCallStageId])
REFERENCES [dbo].[WorkStage] ([WorkStageId])
GO
ALTER TABLE [dbo].[ServiceCall] CHECK CONSTRAINT [FK_ServiceCall_WorkStages]
GO
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK_Task_Activity] FOREIGN KEY([ActivityId])
REFERENCES [dbo].[Activity] ([ActivityId])
GO
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK_Task_Activity]
GO
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK_Task_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK_Task_Status]
GO
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK_Task_Team] FOREIGN KEY([AssignedTeam])
REFERENCES [dbo].[Team] ([TeamId])
GO
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK_Task_Team]
GO
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK_Task_User] FOREIGN KEY([AssignedUser])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK_Task_User]
GO
ALTER TABLE [dbo].[TeamUser]  WITH CHECK ADD  CONSTRAINT [FK_TeamUsers_Team] FOREIGN KEY([TeamId])
REFERENCES [dbo].[Team] ([TeamId])
GO
ALTER TABLE [dbo].[TeamUser] CHECK CONSTRAINT [FK_TeamUsers_Team]
GO
ALTER TABLE [dbo].[TeamUser]  WITH CHECK ADD  CONSTRAINT [FK_TeamUsers_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[TeamUser] CHECK CONSTRAINT [FK_TeamUsers_Users]
GO
ALTER TABLE [dbo].[WorkOrder]  WITH CHECK ADD  CONSTRAINT [FK_WorkOrder_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[WorkOrder] CHECK CONSTRAINT [FK_WorkOrder_Status]
GO
ALTER TABLE [dbo].[WorkOrder]  WITH CHECK ADD  CONSTRAINT [FK_WorkOrder_Team] FOREIGN KEY([AssignedTeam])
REFERENCES [dbo].[Team] ([TeamId])
GO
ALTER TABLE [dbo].[WorkOrder] CHECK CONSTRAINT [FK_WorkOrder_Team]
GO
ALTER TABLE [dbo].[WorkOrder]  WITH CHECK ADD  CONSTRAINT [FK_WorkOrder_Users] FOREIGN KEY([AssignedUser])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[WorkOrder] CHECK CONSTRAINT [FK_WorkOrder_Users]
GO
ALTER TABLE [dbo].[WorkOrder]  WITH CHECK ADD  CONSTRAINT [FK_WorkOrder_WorkStages] FOREIGN KEY([WorkOrderStageId])
REFERENCES [dbo].[WorkStage] ([WorkStageId])
GO
ALTER TABLE [dbo].[WorkOrder] CHECK CONSTRAINT [FK_WorkOrder_WorkStages]
GO
