export interface ProblemDetails {    
    detail: string | null;
    status: number;
    title : string;
};

export interface ValidationProblemDetails extends ProblemDetails {
    errors : Map<string, string[]>
}