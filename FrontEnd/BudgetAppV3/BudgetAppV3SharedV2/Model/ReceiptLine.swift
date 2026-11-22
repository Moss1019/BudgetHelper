//

import Foundation

public class ReceiptLine: Decodable, Encodable {
    public let id: String
    
    public let receiptId: String
    
    public let title: String
    
    public let amount: Decimal
    
    public init(id: String, receiptId: String, title: String, amount: Decimal) {
        self.id = id
        self.receiptId = receiptId
        self.title = title
        self.amount = amount
    }
    
    enum CodingKeys: String, CodingKey {
        case id
        case receiptId
        case title
        case amount
    }
}
