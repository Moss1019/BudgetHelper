import SwiftUI
import BudgetAppV3SharedV2

struct BudgetCard: View {
    let budget: Budget
    
    var body: some View {
        VStack(alignment: .leading, spacing: 12) {
            Text("Monthly Budget")
                .font(.headline)
                .foregroundColor(.primary)
            
            HStack {
                Text(formatCurrency(budget.monthlyLimit))
                    .font(.title)
                    .bold()
                    .foregroundColor(.accentColor)
                
                Spacer()
            }
            
            Text("Total monthly allocation")
                .font(.subheadline)
                .foregroundColor(.secondary)
        }
        .padding()
        .background(
            RoundedRectangle(cornerRadius: 16)
                .fill(.white)
                .shadow(color: .black.opacity(0.1), radius: 8, x: 0, y: 4)
        )
        .contentShape(RoundedRectangle(cornerRadius: 16))
    }
    
    private func formatCurrency(_ amount: Decimal) -> String {
        let formatter = NumberFormatter()
        formatter.numberStyle = .currency
        formatter.locale = Locale.current
        return formatter.string(from: NSDecimalNumber(decimal: amount)) ?? "$\(amount)"
    }
}
