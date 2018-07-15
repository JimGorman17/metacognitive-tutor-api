using ServiceStack.WebHost.Endpoints;
using MetacognitiveTutor.Api.Services;
using PetaPoco;
using Funq;
using MetacognitiveTutor.DataLayer.Models;
using MetacognitiveTutor.DataLayer.Repositories;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Net;
using ServiceStack.Text;
using ServiceStack;
using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface.Cors;
using ServiceStack.WebHost.Endpoints.Support;
using System.Web;
using System;
using ServiceStack.WebHost.Endpoints.Extensions;
using System.Configuration;

[assembly: WebActivator.PreApplicationStartMethod(typeof(MetacognitiveTutor.Api.App_Start.AppHost), "Start")]


/**
 * Auto-Generated Metadata API page at: /metadata
 * See other complete web service examples at: https://github.com/ServiceStack/ServiceStack.Examples
 */

namespace MetacognitiveTutor.Api.App_Start
{
    public class AppHost
		: AppHostBase
	{		
		public AppHost() //Tell ServiceStack the name and where to find your web services
			: base("MetacognitiveTutor.Api", typeof(UserService).Assembly) { }

		public override void Configure(Container container)
		{
            //Set JSON web services to return idiomatic JSON camelCase properties
		    JsConfig.EmitCamelCaseNames = true;
            JsConfig.ThrowOnDeserializationError = true;
		    JsConfig.DateHandler = JsonDateHandler.ISO8601;
		    JsConfig<Guid>.SerializeFn = guid => guid.ToString("D");
		    JsConfig<Guid?>.SerializeFn = nullableGuid => nullableGuid.HasValue ? nullableGuid.Value.ToString("D") : string.Empty;

		    Plugins.Add(new CorsFeature(ConfigurationManager.AppSettings["webSiteDomain"], "GET, POST, PUT, DELETE, OPTIONS", "Content-Type"));

		    //Handles Request and closes Response after emitting global HTTP Headers
		    var emitGlobalHeadersHandler = new CustomActionHandler(
		        (httpReq, httpRes) => httpRes.EndRequest());

		    SetConfig(new EndpointHostConfig
		    {
		        RawHttpHandlers = { httpReq =>
		            httpReq.HttpMethod == HttpMethods.Options
		                ? emitGlobalHeadersHandler
		                : null  }
#if !DEBUG
                , DebugMode = false   // Remove stacktrace inside the response
#endif
		    });
            
            container.Register(c => new Database("localDB")).ReusedWithin(ReuseScope.Request);
		    container.RegisterAutoWired<Repository<ErrorLog>>().ReusedWithin(ReuseScope.Request);
		    container.RegisterAutoWired<Repository<Lesson>>().ReusedWithin(ReuseScope.Request);
            container.RegisterAutoWired<UserRepository>().ReusedWithin(ReuseScope.Request);
            
            ServiceExceptionHandler = (req, request, exception) =>
		    {
		        var errorLog = new ErrorLog
		        {
		            Application = "MetacognitiveTutor.Api",
		            Message = exception.Message,
		            StackTrace = exception.StackTrace
		        };
		        container.Resolve<Repository<ErrorLog>>().Add(errorLog);

		        return DtoUtils.CreateErrorResponse(request, exception, new ResponseStatus(HttpStatusCode.InternalServerError.ToString()));
		    };

		    //Handle Unhandled Exceptions occurring outside of Services
		    //E.g. Exceptions during Request binding or in filters:
		    ExceptionHandler = (req, res, operationName, ex) =>
		    {
		        var errorLog = new ErrorLog
		        {
		            Application = "MetacognitiveTutor.Api",
		            Message = ex.Message,
		            StackTrace = ex.StackTrace
		        };
		        container.Resolve<Repository<ErrorLog>>().Add(errorLog);

		        res.Write("Error: {0}: {1}".Fmt(ex.GetType().Name, ex.Message));
		        res.EndRequest(skipHeaders: true);
		    };

            //Enable Authentication
            //ConfigureAuth(container);
		}

		/* Uncomment to enable ServiceStack Authentication and CustomUserSession
		private void ConfigureAuth(Funq.Container container)
		{
			var appSettings = new AppSettings();

			//Default route: /auth/{provider}
			Plugins.Add(new AuthFeature(() => new CustomUserSession(),
				new IAuthProvider[] {
					new CredentialsAuthProvider(appSettings), 
					new FacebookAuthProvider(appSettings), 
					new TwitterAuthProvider(appSettings), 
					new BasicAuthProvider(appSettings), 
				})); 

			//Default route: /register
			Plugins.Add(new RegistrationFeature()); 

			//Requires ConnectionString configured in Web.Config
			var connectionString = ConfigurationManager.ConnectionStrings["AppDb"].ConnectionString;
			container.Register<IDbConnectionFactory>(c =>
				new OrmLiteConnectionFactory(connectionString, SqlServerDialect.Provider));

			container.Register<IUserAuthRepository>(c =>
				new OrmLiteAuthRepository(c.Resolve<IDbConnectionFactory>()));

			var authRepo = (OrmLiteAuthRepository)container.Resolve<IUserAuthRepository>();
			authRepo.CreateMissingTables();
		}
		*/

		public static void Start()
		{
			new AppHost().Init();
		}
	}

    public class CustomActionHandler : IServiceStackHttpHandler, IHttpHandler
    {
        public Action<IHttpRequest, IHttpResponse> Action { get; set; }

        public CustomActionHandler(Action<IHttpRequest, IHttpResponse> action)
        {
            if (action == null)
            {
                throw new Exception("Action was not supplied to ActionHandler");
            }

            Action = action;
        }

        public void ProcessRequest(IHttpRequest httpReq, IHttpResponse httpRes, string operationName)
        {
            Action(httpReq, httpRes);
        }

        public void ProcessRequest(HttpContext context)
        {
            ProcessRequest(context.Request.ToRequest(GetType().Name),
                context.Response.ToResponse(),
                GetType().Name);
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}
