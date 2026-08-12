export type ValidationErrors = Record<string, string[]>;

export interface ApiError {
  status: number;
  message: string;
}