export interface ProblemDetails {    
    detail: string;
    status: number;
    title : string;
};

export interface ValidationProblemDetails extends ProblemDetails {
    errors : Record<string, string[]>
}