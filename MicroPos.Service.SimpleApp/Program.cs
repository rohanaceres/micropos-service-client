#if DEBUG
using MicroPos.Service.SimpleApp.DebugReference;
#else
using MicroPos.Service.SimpleApp.DebugReference;
#endif
using System;

namespace MicroPos.Service.SimpleApp
{
    class Program
    {
        static IMicroPosService service = new MicroPosServiceClient();

        static void Main(string[] args)
        {
			DisplayableMessagesContract messages = new DisplayableMessagesContract();
			messages.ApprovedMessage = "^.^";
			messages.DeclinedMessage = "-_-";
			messages.InitializationMessage = "Ei!";
			messages.MainLabel = "Hola, que tal?";
			messages.ProcessingMessage = "Pensando...";

			GetOneOrFirstPinpadRequest pinpadRequest = new GetOneOrFirstPinpadRequest();
            pinpadRequest.AuthorizerUri = ""; // preencher com a URL do autorizador da Stone
            pinpadRequest.SaleAffiliationKey = ""; // preencher com o SAK
            pinpadRequest.TmsUri = ""; // preencher com a URL do TMS da Stone
			pinpadRequest.PinpadMessages = messages;

            GetOneOrFirstPinpadResponse pinpadResponse = service.GetOneOrFirstPinpad(pinpadRequest);

            AuthorizationRequest authRequest = new AuthorizationRequest();
            authRequest.CardPaymentAuthorizer = pinpadResponse.CardPaymentAuthorizer;
            authRequest.Transaction = new TransactionContract();
            authRequest.Transaction.Amount = 0.1m;
			authRequest.Language = "pt-BR";
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

            ClosePinpadConnectionRequest closeRequest = new ClosePinpadConnectionRequest();
            closeRequest.CardPaymentAuthorizer = pinpadResponse.CardPaymentAuthorizer;
            ClosePinpadConnectionResponse closeResponse = service.ClosePinpadConnection(closeRequest);
        }
    }
}
