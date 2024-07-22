/*export async function sendReq(path: string, headers: object | undefined = undefined) {
    let response; //:Promise<Response>
    if (headers) {
        response = await fetch(path, headers);
    }
    else {
        response = await fetch(path);
    }
    const data = await response.json();
    return data;
}
*/