namespace LearnHub.Back.Domain;

public class Payment
{
    public Guid Id { get; set; }
    public Guid EnrollmentId { get; set; }
    public Enrollment Enrollment { get; set; }
    public string PaymentMethod { get; set; } // Credit Card, PayPal, etc.
    public string CardNumber { get; set; }
    public DateTime CardExpirationDate { get; set; }
    public string CVV { get; set; }
    public decimal PaymentAmount { get; set; }
    public DateTime PaymentDate { get; set; }
}