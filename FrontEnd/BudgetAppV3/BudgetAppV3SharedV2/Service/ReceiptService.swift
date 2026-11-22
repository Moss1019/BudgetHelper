import SwiftUI

public class ReceiptService {
    let client = ReceiptClient()
    
    public init() {}
    
    public func getAll() async -> OpResult<[Receipt]> {
        await client.getAll()
    }
    
    public func create(_ receipt: Receipt) async -> OpResult<Receipt> {
        await client.create(receipt)
    }
}
