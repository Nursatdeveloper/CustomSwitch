// See https://aka.ms/new-console-template for more information
using CustomSwitch;

namespace CustomSwitch {
    public record PaymentResult(int paymentId, int paymentAmout, PaymentStatus status);
    public class CreateEmpOp {
        public int EmpOpId { get; set; }
        public int EmpId { get; set; }
        public string EmpName { get; set; }
    }
    public class EditEmpOp {
        public string Name { get; set; }
    }
    public enum EmpOpType {
        Create,
        Edit,
        Delete
    }

    public enum PaymentStatus {
        Success, Fail, Processing
    }
    class Program {
        static void Main(string[] args) {
            var empOpType = EmpOpType.Create;
            var customSwitch = new CustomSwitchBuilderV1<EmpOpType>(empOpType);
            customSwitch
                .Case(EmpOpType.Create)
                    .Then(x => {
                        Console.WriteLine("There Will be emp create op");
                    })
                .Case(EmpOpType.Edit)
                    .Then(x => {
                        Console.WriteLine("There will be emp edit op");
                    })
                .Case(EmpOpType.Delete)
                    .Then(x => {
                        Console.WriteLine("Emp deleted");
                    })
                .Default(x => {
                    Console.WriteLine("Default action executed");
                });

            var switchResult = new SwitchTBuilder<EmpOpType>(empOpType)
                                    .Case(EmpOpType.Create)
                                        .ThenT<CreateEmpOp>(f => {
                                            var createEmpOp = new CreateEmpOp {
                                                EmpId = 1,
                                                EmpName = "Nursat"
                                            };
                                            return createEmpOp;
                                        })
                                    .Case(EmpOpType.Edit)
                                        .ThenT<EditEmpOp>(f => {
                                            var editEmpOp = new EditEmpOp {
                                                Name = "Kasym"
                                            };
                                            return editEmpOp;
                                        })
                                    .DefaultT<string>(x => {
                                        return "Not found";
                                    })
                                    .Build();

            Console.WriteLine($"{switchResult} Type: {switchResult.GetType()}");

            PaymentStatus paymentStatus = PaymentStatus.Processing;

            var paymentResult = new SwitchTBuilder<PaymentStatus>(paymentStatus)
                                .Case(PaymentStatus.Success)
                                    .ThenT<PaymentResult>(f => {
                                        return new PaymentResult(1, 12000, PaymentStatus.Success);
                                    })
                                .Case(PaymentStatus.Fail)
                                    .ThenT<PaymentResult>(f => {
                                        return new PaymentResult(1, 12000, PaymentStatus.Fail);
                                    })
                                .DefaultT<PaymentResult>(x => {
                                    Console.WriteLine("Payment seems to be processing. Please wait couple of seconds...");
                                    return new PaymentResult(1, 12000, PaymentStatus.Processing);
                                })
                                .BuildT();

            Console.WriteLine($"PaymentId: {paymentResult.paymentId}\nAmount: {paymentResult.paymentAmout}\nStatus: {paymentResult.status.ToString()}");

        }
    }
}






