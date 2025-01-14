export interface assignment {
    taskId: number;           
    title: string;
    description: string;   
    isCompleted?: boolean;
    categoryName? : string;
    dueDate : Date;
    createdAt?: Date;
    updatedAt?: Date;
}