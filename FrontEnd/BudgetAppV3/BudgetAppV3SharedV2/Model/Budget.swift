import Foundation

public class Budget: Decodable, Encodable {
    public let id: Int
    public var monthlyLimit: Decimal
    
    public init(id: Int, monthlyLimit: Decimal) {
        self.id = id
        self.monthlyLimit = monthlyLimit
    }
}
