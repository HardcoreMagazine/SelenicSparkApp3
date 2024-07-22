import { ApiEndpoints } from './EApiEndpoints'
import { HttpMethods } from './EHttpMethods';
import { StatusCodes } from './EStatusCodes'

interface IApiRequest {
  endpoint: ApiEndpoints;
  method: HttpMethods;
  params?: string;
  body?: any;
  afterHandler?: (response: any) => void;
}

export class ApiService {
  private static async handleResponse(res: Response): Promise<any> {
    switch (res.status) {
      case 400: // bad request
        return StatusCodes.ClientFail;
      case 500: // internal server err
        return StatusCodes.ServerFail;
      default: // blindly guessing response code will be "200/OK" in all other cases
        const resData = await res.json();
        return resData;
    }
  }

  public static async handleRequest(request: IApiRequest): Promise<any> {
    let url: string;

    if (request.params) {
      url = `${request.endpoint}${request.params}`;
    }
    else {
      url = request.endpoint;
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
