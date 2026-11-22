import Foundation

class ApiClient {
    // TODO: Is it possible to get an environment variable in here
    private let baseUrl = "http://192.168.50.149:3000"
    
    private let decoder = JSONDecoder()
    
    private let encoder = JSONEncoder()
    
    // Create a dedicated session with configuration
    private let session: URLSession
    
    init() {
        decoder.dateDecodingStrategy = .iso8601
        encoder.dateEncodingStrategy = .iso8601
        
        // Create a session configuration with timeout and cancellation support
        let config = URLSessionConfiguration.default
        config.timeoutIntervalForRequest = 30 // 30 seconds for each request
        config.timeoutIntervalForResource = 60 // 60 seconds for total request
        self.session = URLSession(configuration: config)
    }
    
    // Ensure the session is properly invalidated
    deinit {
        session.invalidateAndCancel()
    }
    
    func get<T: Decodable>(_ type: T.Type, remoteUrl: String) async -> OpResult<T> {
        guard let url = URL(string: "\(baseUrl)/\(remoteUrl)") else {
            return OpResult.failed(message: "Invalid URL")
        }
        
        var req = URLRequest(url: url)
        req.httpMethod = "GET"
        
        do {
            // Use the dedicated session
            let (data, res) = try await session.data(for: req)
            guard let httpRes = res as? HTTPURLResponse,
                  (200...299).contains(httpRes.statusCode) else {
                return OpResult.failed(message: "A remote error occured during this request")
            }
            
            guard !data.isEmpty else {
                return OpResult.failed(message: "Received empty response from server")
            }
            
            print(String(bytes: data, encoding: .utf8))
            
            // Use the configured decoder
            let decoded = try decoder.decode(T.self, from: data)
            return OpResult.success(content: decoded)
        } catch {
            // Handle different error types appropriately
            let errorMessage: String
            
            if let urlError = error as? URLError {
                switch urlError.code {
                case .cancelled:
                    errorMessage = "Request was cancelled"
                case .timedOut:
                    errorMessage = "Request timed out"
                case .notConnectedToInternet:
                    errorMessage = "No internet connection"
                default:
                    errorMessage = "Network error: \(urlError.localizedDescription)"
                }
            } else {
                errorMessage = "Decoding error: \(error.localizedDescription)"
            }
            
            print("API error: \(errorMessage)")
            return OpResult.failed(message: errorMessage)
        }
    }
    
    func post<T: Decodable, V: Encodable>(_ type: T.Type, remoteUrl: String, value: V) async -> OpResult<T> {
        guard let url = URL(string: "\(baseUrl)/\(remoteUrl)") else {
            return OpResult.failed(message: "Invalid URL")
        }
        
        var req = URLRequest(url: url)
        req.httpMethod = "POST"
        req.setValue("application/json", forHTTPHeaderField: "Content-Type")
        req.setValue("application/json", forHTTPHeaderField: "Accept")
        
        do {
            req.httpBody = try encoder.encode(value)
            
            // Use the dedicated session
            let (data, res) = try await session.data(for: req)
            guard let httpRes = res as? HTTPURLResponse,
                  (200...299).contains(httpRes.statusCode) else {
                return OpResult.failed(message: "A remote error has occured during the request")
            }
            
            guard !data.isEmpty else {
                return OpResult.failed(message: "Received empty response from server")
            }
            
            let decoded = try decoder.decode(T.self, from: data)
            return OpResult.success(content: decoded)
        } catch {
            let errorMessage: String
            
            if let urlError = error as? URLError {
                switch urlError.code {
                case .cancelled:
                    errorMessage = "Request was cancelled"
                case .timedOut:
                    errorMessage = "Request timed out"
                default:
                    errorMessage = "Network error: \(urlError.localizedDescription)"
                }
            } else {
                errorMessage = "Post error: \(error.localizedDescription)"
            }
            
            print(errorMessage)
            return OpResult.failed(message: "Could not connect to remote server \(url)")
        }
    }
    
    func put<T: Decodable, V: Encodable>(_ type: T.Type, remoteUrl: String, value: V) async -> OpResult<T> {
        guard let url = URL(string: "\(baseUrl)/\(remoteUrl)") else {
            return OpResult.failed(message: "Invalid URL")
        }
        
        var req = URLRequest(url: url)
        req.httpMethod = "PUT"
        
        do {
            req.httpBody = try encoder.encode(value)
            
            // Use the dedicated session
            let (data, res) = try await session.data(for: req)
            guard let httpRes = res as? HTTPURLResponse,
                  (200...299).contains(httpRes.statusCode) else {
                return OpResult.failed(message: "A remote error has occured during the request")
            }
            
            guard !data.isEmpty else {
                return OpResult.failed(message: "Received empty response from server")
            }
            
            let decoded = try decoder.decode(T.self, from: data)
            return OpResult.success(content: decoded)
        } catch {
            let errorMessage: String
            
            if let urlError = error as? URLError {
                switch urlError.code {
                case .cancelled:
                    errorMessage = "Request was cancelled"
                case .timedOut:
                    errorMessage = "Request timed out"
                default:
                    errorMessage = "Network error: \(urlError.localizedDescription)"
                }
            } else {
                errorMessage = "Put error: \(error.localizedDescription)"
            }
            
            print(errorMessage)
            return OpResult.failed(message: "Could not connect to remote server \(url)")
        }
    }
}
