import SwiftUI
import BudgetAppV3SharedV2

struct ReceiptLineCard: View {
    let line: ReceiptLine
    
    var body: some View {
        VStack(alignment: .leading, spacing: 12) {
            Text(line.title)
                .font(.headline)
                .foregroundColor(.primary)
            
            HStack {
                Text("Amount")
                    .font(.subheadline)
                    .foregroundColor(.secondary)
                
                Spacer()
                
                Text(formatCurrency(line.amount))
                    .font(.headline)
                    .bold()
                    .foregroundColor(.accentColor)
            }
            
            Text("Line #\(String(line.id.prefix(8)))")
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
    
    private func formatCurrency(_ amount: Decimal) -> String {
        let formatter = NumberFormatter()
        formatter.numberStyle = .currency
        formatter.locale = Locale.current
        return formatter.string(from: NSDecimalNumber(decimal: amount)) ?? "$\(amount)"
    }
}
