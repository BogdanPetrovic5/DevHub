import { RepoDto } from './repo.model';

export interface ProfileDto {
    username: string;
    firstName: string;
    lastName: string;
    email: string;
    repoCount: number;
    commitCount: number;
    repositories: RepoDto[];
}
