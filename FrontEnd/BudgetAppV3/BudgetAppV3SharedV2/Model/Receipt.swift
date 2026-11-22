import Foundation

public class Receipt: Decodable, Encodable {
    public let id: String
    public let date: String
    public let amount: Decimal
    
    public init(id: String, date: String, amount: Decimal) {
        self.id = id
        self.date = date
        self.amount = amount
    }
    
    enum CodingKeys: String, CodingKey {
        case id
        case date
        case amount
    }
}

