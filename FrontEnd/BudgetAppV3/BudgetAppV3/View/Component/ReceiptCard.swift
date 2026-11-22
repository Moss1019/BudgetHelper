import SwiftUI
import BudgetAppV3SharedV2

struct ReceiptCard: View {
    let receipt: Receipt
    
    var body: some View {
        VStack(alignment: .leading, spacing: 12) {
            HStack {
                Text(receipt.date)
                    .font(.subheadline)
                    .foregroundColor(.secondary)
                
                Spacer()
                
                Text(formatCurrency(receipt.amount))
                    .font(.headline)
                    .bold()
                    .foregroundColor(.accentColor)
            }
            
            // Show a summary of receipt lines if needed (optional)
            Text("Receipt #\(String(receipt.id.prefix(8)))")
                .font(.caption)
                .foregroundColor(.secondary)
        }
        .padding()
        .background(
            RoundedRectangle(cornerRadius: 12)
                .fill(.white)
                .shadow(color: .black.opacity(0.08), radius: 6, x: 0, y: 3)
        )
    }
    
    private func formatDate(_ date: Date) -> String {
        let formatter = DateFormatter()
        formatter.dateStyle = .medium
        return formatter.string(from: date)
    }
    
    private func formatCurrency(_ amount: Decimal) -> String {
        let formatter = NumberFormatter()
        formatter.numberStyle = .currency
        formatter.locale = Locale.current
        return formatter.string(from: NSDecimalNumber(decimal: amount)) ?? "$\(amount)"
    }
}
