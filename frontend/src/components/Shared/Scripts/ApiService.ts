import { ApiEndpoints } from './ApiEndpoints'
import { BasicHttpMethods } from './HttpMethods';
import { ApiEndpointActions } from './ApiEndpointActions';

interface IApiRequest {
  endpoint: ApiEndpoints;
  action: ApiEndpointActions;
  method: BasicHttpMethods;
  params?: string;
  body?: any;
  afterHandler?: (response: any) => void;
}

export class ApiService {
  private static async handleResponse(res: Response): Promise<any> {
    switch (res.status) {
      case 200:
        const resData = await res.json();
        return resData;
      default:
        return "EPIC FAIL";
    }
  }

  public static async handleRequest(request: IApiRequest): Promise<any> {
    let url: string;

    if (!request.action) {
      url = request.endpoint;
    }
    else if (!request.params) {
      url = `${request.endpoint}${request.action}`;
    }
    else {
      url = `${request.endpoint}${request.action}${request.params}`;
    }

    const response = await fetch(url, {
      method: request.method,
      headers: request.body ? { 'Content-Type': 'application/json' } : undefined,
      body: request.body ? request.body : undefined
    });

    const data = await this.handleResponse(response);

    if (request.afterHandler) {
      request.afterHandler(data);
    }

    return data;
  }
}
