using MicroPos.Service.SimpleApp.ServiceReference2;
using System;
using System.Configuration;

namespace MicroPos.Service.SimpleApp
{
    class Program
    {
        static IMicroPosService service = new MicroPosServiceClient();

        static void Main(string[] args)
        {
            GetOneOrFirstPinpadRequest pinpadRequest = new GetOneOrFirstPinpadRequest();
            pinpadRequest.AuthorizerUri = "AuthorizerUri";
            pinpadRequest.Language = "pt-BR";
            pinpadRequest.SaleAffiliationKey = "SAK";
            pinpadRequest.TmsUri = "TmsUri";

            GetOneOrFirstPinpadResponse pinpadResponse = service.GetOneOrFirstPinpad(pinpadRequest);

            AuthorizationRequest authRequest = new AuthorizationRequest();
            authRequest.CardPaymentAuthorizer = pinpadResponse.CardPaymentAuthorizer;
            authRequest.Language = "pt-BR";
            authRequest.Transaction = new TransactionContract();
            authRequest.Transaction.Amount = 0.1m;
            authRequest.Transaction.CaptureTransaction = true;
            authRequest.Transaction.InitiatorTransactionKey = Guid.NewGuid().ToString();
            authRequest.Transaction.Type = "Debit";

            AuthorizationResponse authResponse = service.Authorize(authRequest);

            if (authResponse.Failure == true)
            {
                // Operação falhou:
            }
            else if (authResponse.AuthorizationReport.WasApproved == false)
            {
                // Se o processo de autorização falhou
            }
            else
            {
                // Operação bem sucedida e transação autorizada!
            }
        }
    }
}
