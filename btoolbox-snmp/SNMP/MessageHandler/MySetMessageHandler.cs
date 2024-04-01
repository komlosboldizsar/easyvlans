using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Pipeline;

namespace BToolbox.SNMP
{
    // copied from Lextm.SharpSnmpLib.Pipeline.SetMessageHandler
    public sealed class MySetMessageHandler : IMessageHandler
    {
        public void Handle(ISnmpContext context, ObjectStore store)
        {

            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (store == null)
                throw new ArgumentNullException(nameof(store));

            context.CopyRequest(ErrorCode.InconsistentName, int.MaxValue);
            if (context.TooBig)
            {
                context.GenerateTooBig();
                return;
            }

            int num = 0;
            ErrorCode result = ErrorCode.NoError;
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
                        result = ErrorCode.NoAccess;
                    }
                    catch (ArgumentException ex)
                    {
                        if (!Enum.TryParse(ex.Message, out result) || (result == ErrorCode.NoError))
                            result = ErrorCode.WrongType;
                    }
                    catch (SnmpErrorCodeException ex)
                    {
                        result = ex.ErrorCode;
                    }
                    catch (Exception)
                    {
                        result = ErrorCode.GenError;
                    }
                }
                else
                {
                    result = ErrorCode.NotWritable;
                }

                if (result != 0)
                {
                    context.CopyRequest(result, num);
                    return;
                }

                list.Add(variable);
            }

            context.GenerateResponse(list);

        }
    }
}
