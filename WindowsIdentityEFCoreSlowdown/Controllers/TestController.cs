using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;
using WindowsIdentityEFCoreSlowdown.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using System.Text;

namespace WindowsIdentityEFCoreSlowdown.Controllers {
	public class TestController : Controller {
		[Route("")]
		public ActionResult Test() {
			WindowsIdentity identity = this.HttpContext.User.Identity as WindowsIdentity;
			if (identity == null) {
				return this.StatusCode(401);
			}

			StringBuilder output = new StringBuilder();

			output.AppendLine($"Doing some stuff as {WindowsIdentity.GetCurrent().Name}...");

			DateTime start = DateTime.Now;
			using (TestContext test = new TestContext()) {
				test.Tests.Any();
				test.Tests.ToArray();
				test.Tests.Where(x => x.Id > 2).Select(x => x.Id).ToArray();
			}

			output.AppendLine($"Time taken: {DateTime.Now - start}");

			output.AppendLine($"Doing some stuff as {identity.Name}...");

			start = DateTime.Now;
			WindowsIdentity.RunImpersonated(identity.AccessToken, () => {
				using (TestContext test = new TestContext()) {
					test.Tests.Any();
					test.Tests.ToArray();
					test.Tests.Where(x => x.Id > 2).Select(x => x.Id).ToArray();
				}
			});

			output.AppendLine($"Time taken: {DateTime.Now - start}");

			return this.Content(output.ToString());
		}
	}
}
