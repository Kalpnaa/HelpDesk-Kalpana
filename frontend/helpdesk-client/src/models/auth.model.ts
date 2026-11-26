export interface RegisterRequest
{
    fullname : string;
    email :string;
    password : string;
}

export interface LoginRequest
{
    email: string;
    password : string;
}

export interface RegisterResponse
{
    email : string;
    password : string;
}
export interface LoginResponse
{
    id: number;
    fullname : string;
    email: string;
    role : string;
    token : string;
}