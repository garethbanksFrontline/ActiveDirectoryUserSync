﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ActiveDirectoryServices
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="FrontlineActiveDirectoryTest")]
	public partial class UpdateUsersDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    #endregion
		
		public UpdateUsersDataContext() : 
				base(global::ActiveDirectoryServices.Properties.Settings.Default.FrontlineActiveDirectoryTestConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public UpdateUsersDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public UpdateUsersDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public UpdateUsersDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public UpdateUsersDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.InsertOrUpdateUser")]
		public int InsertOrUpdateUser([global::System.Data.Linq.Mapping.ParameterAttribute(Name="Email", DbType="VarChar(500)")] string email, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="UserName", DbType="VarChar(500)")] string userName, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="DisplayName", DbType="VarChar(500)")] string displayName, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Department", DbType="VarChar(500)")] string department, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Division", DbType="VarChar(500)")] string division, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="IsMapped", DbType="Bit")] System.Nullable<bool> isMapped, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="AccountControl", DbType="VarChar(500)")] string accountControl, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Manager", DbType="VarChar(500)")] string manager, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Timesheet", DbType="VarChar(50)")] string timesheet)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), email, userName, displayName, department, division, isMapped, accountControl, manager, timesheet);
			return ((int)(result.ReturnValue));
		}
	}
}
#pragma warning restore 1591
