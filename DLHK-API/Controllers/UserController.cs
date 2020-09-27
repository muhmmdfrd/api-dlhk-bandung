﻿using Core.Manager.UserManager;
using DLHK_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace DLHK_API.Controllers
{
	public class UserController : ApiController
    {
		private readonly ApiResponse<NameForClaim> resp = new ApiResponse<NameForClaim>();
		private readonly ApiResponse<List<UserDTO>> respList = new ApiResponse<List<UserDTO>>();

		[HttpGet]
		[Route("api/user")]
		public IHttpActionResult Get()
		{
			try
			{
				using (var manager = new UserAdapter())
				{
					respList.Message = "data found";
					respList.MessageCode = 200;
					respList.ErrorCode = 0;
					respList.Data = manager.Query.Value.Tranform();
				}
				 
			}
			catch (Exception ex)
			{
				respList.Message = ex.Message;
				respList.MessageCode = 400;
				respList.ErrorCode = 1;
				respList.Data = null;
			}

			return Json(respList);
		}

		[HttpGet]
		[Route("api/user/claim")]
		public IHttpActionResult GetClaim()
		{
			try
			{
				var identity = (ClaimsIdentity)User.Identity;

				var newResponse = new NameForClaim
				{
					RoleName = ClaimIdentityType(identity, ClaimTypes.Role),
					Name = ClaimIdentityType(identity, ClaimTypes.Name),
					Photo = ClaimIdentityType(identity, "Photo"),
					RegionName = ClaimIdentityType(identity, "Region"),
					ZoneName = ClaimIdentityType(identity, "Zone"),
					UserId = ClaimIdentityType(identity, "UserId"),
					Shift = ClaimIdentityType(identity, "Shift")
				};

				resp.Message = "data found";
				resp.MessageCode = 200;
				resp.ErrorCode = 0;
				resp.Data = newResponse;
			}
			catch (Exception ex)
			{
				resp.Message = ex.Message;
				resp.MessageCode = 400;
				resp.ErrorCode = 1;
				resp.Data = null;
			}

			return Json(resp);
		}

		private class NameForClaim
		{
			public string Name { get; set; }
			public string RoleName { get; set; }
			public string ZoneName { get; set; }
			public string RegionName { get; set; }
			public string Photo { get; set; }
			public string UserId { get; set; }
			public string Shift { get; set; }
		}

		private string ClaimIdentityType(ClaimsIdentity identity, string type)
		{
			return string.Join(",", identity.Claims.Where(s => s.Type.Equals(type)).Select(s => s.Value).ToList());
		}
	}
}
