using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Pipeline;

namespace BToolbox.SNMP
{
    // copied from Lextm.SharpSnmpLib.Pipeline.SetV1MessageHandler
    internal sealed class MySetV1MessageHandler : IMessageHandler
    {
        public void Handle(ISnmpContext context, ObjectStore store)
        {

            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (store == null)
                throw new ArgumentNullException(nameof(store));
            int num = 0;
            ErrorCode errorCode = ErrorCode.NoError;
            IList<Variable> list = new List<Variable>();

            foreach (Variable variable in context.Request.Pdu().Variables)
            {
                num++;
                ScalarObject @object = store.GetObject(variable.Id);
                if (@object != null)
                {
                    try
                    {
                        @object.Data = variable.Data;
                    }
                    catch (AccessFailureException)
                    {
                        errorCode = ErrorCode.NoSuchName;
                    }
                    catch (ArgumentException)
                    {
                        errorCode = ErrorCode.BadValue;
                    }
                    catch (SnmpErrorCodeException ex)
                    {
                        errorCode = v2ErrorCoreToV1(ex.ErrorCode);
                    }
                    catch (Exception)
                    {
                        errorCode = ErrorCode.GenError;
                    }
                }
                else
                {
                    errorCode = ErrorCode.NoSuchName;
                }

                if (errorCode != 0)
                {
                    context.CopyRequest(errorCode, num);
                    return;
                }

                list.Add(variable);

            }

            context.GenerateResponse(list);

        }

        private ErrorCode v2ErrorCoreToV1(ErrorCode errorCode)
        {
            // @source https://datatracker.ietf.org/doc/rfc2089/
            return errorCode switch
            {
                ErrorCode.WrongValue => ErrorCode.BadValue,
                ErrorCode.WrongEncoding => ErrorCode.BadValue,
                ErrorCode.WrongType => ErrorCode.BadValue,
                ErrorCode.WrongLength => ErrorCode.BadValue,
                ErrorCode.InconsistentValue => ErrorCode.BadValue,
                ErrorCode.NoAccess => ErrorCode.NoSuchName,
                ErrorCode.NotWritable => ErrorCode.NoSuchName,
                ErrorCode.NoCreation => ErrorCode.NoSuchName,
                ErrorCode.InconsistentName => ErrorCode.NoSuchName,
                ErrorCode.ResourceUnavailable => ErrorCode.GenError,
                ErrorCode.CommitFailed => ErrorCode.GenError,
                ErrorCode.UndoFailed => ErrorCode.GenError,
                ErrorCode.AuthorizationError => ErrorCode.GenError,
                _ => errorCode
            };
        }

    }
}
