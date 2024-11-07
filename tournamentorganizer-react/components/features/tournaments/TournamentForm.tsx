import React from "react";
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { Textarea } from "@/components/ui/textarea";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { Switch } from "@/components/ui/switch";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import { CreateTournamentRequest, TournamentFormat } from "@/types";

const tournamentFormSchema = z.object({
  name: z
    .string()
    .min(3, "Tournament name must be at least 3 characters")
    .max(100, "Tournament name must be less than 100 characters"),
  description: z.string().optional(),
  format: z.nativeEnum(TournamentFormat),
  startDate: z.string().refine((date) => new Date(date) > new Date(), {
    message: "Start date must be in the future",
  }),
  maxParticipants: z
    .number()
    .min(2, "Tournament must have at least 2 participants")
    .max(128, "Tournament cannot have more than 128 participants"),
  hasPrizePool: z.boolean().default(false),
  prizePool: z.number().min(0, "Prize pool cannot be negative").optional(),
  prizeCurrency: z.string().optional(),
});

type TournamentFormData = z.infer<typeof tournamentFormSchema>;

interface TournamentFormProps {
  onSubmit: (data: CreateTournamentRequest) => Promise<void>;
  isLoading?: boolean;
}

export function TournamentForm({ onSubmit, isLoading }: TournamentFormProps) {
  // Get tomorrow's date at current time
  const tomorrow = new Date();
  tomorrow.setDate(tomorrow.getDate() + 1);
  const tomorrowString = tomorrow.toISOString().slice(0, 16); // Format: YYYY-MM-DDTHH:mm

  const form = useForm<TournamentFormData>({
    resolver: zodResolver(tournamentFormSchema),
    defaultValues: {
      name: "",
      description: "",
      format: TournamentFormat.SingleElimination,
      startDate: tomorrowString,
      maxParticipants: 16,
      hasPrizePool: false,
      prizePool: 0,
      prizeCurrency: "USD",
    },
  });

  const hasPrizePool = form.watch("hasPrizePool");

  const handleSubmit = async (data: TournamentFormData) => {
    const submitData: CreateTournamentRequest = {
      name: data.name,
      description: data.description,
      format: data.format,
      startDate: new Date(data.startDate).toISOString(),
      maxParticipants: data.maxParticipants,
      prizePool: data.hasPrizePool ? data.prizePool : 0,
      prizeCurrency: data.hasPrizePool ? data.prizeCurrency : "USD",
    };

    await onSubmit(submitData);
  };

  return (
    <Card className="w-full max-w-2xl mx-auto">
      <CardHeader>
        <CardTitle>Create New Tournament</CardTitle>
      </CardHeader>
      <CardContent>
        <Form {...form}>
          <form
            onSubmit={form.handleSubmit(handleSubmit)}
            className="space-y-6"
          >
            <FormField
              control={form.control}
              name="name"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Tournament Name</FormLabel>
                  <FormControl>
                    <Input placeholder="Enter tournament name" {...field} />
                  </FormControl>
                  <FormDescription>
                    Choose a unique and descriptive name
                  </FormDescription>
                  <FormMessage />
                </FormItem>
              )}
            />

            <FormField
              control={form.control}
              name="description"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Description (Optional)</FormLabel>
                  <FormControl>
                    <Textarea
                      placeholder="Enter tournament description"
                      className="resize-none"
                      {...field}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <FormField
              control={form.control}
              name="format"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Tournament Format</FormLabel>
                  <Select
                    onValueChange={(value) => field.onChange(Number(value))}
                    defaultValue={field.value.toString()}
                  >
                    <FormControl>
                      <SelectTrigger>
                        <SelectValue placeholder="Select format" />
                      </SelectTrigger>
                    </FormControl>
                    <SelectContent>
                      <SelectItem
                        value={TournamentFormat.SingleElimination.toString()}
                      >
                        Single Elimination
                      </SelectItem>
                      <SelectItem
                        value={TournamentFormat.DoubleElimination.toString()}
                      >
                        Double Elimination
                      </SelectItem>
                      <SelectItem
                        value={TournamentFormat.RoundRobin.toString()}
                      >
                        Round Robin
                      </SelectItem>
                    </SelectContent>
                  </Select>
                  <FormDescription>
                    Choose the tournament bracket format
                  </FormDescription>
                  <FormMessage />
                </FormItem>
              )}
            />

            <FormField
              control={form.control}
              name="startDate"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Start Date</FormLabel>
                  <FormControl>
                    <Input
                      type="datetime-local"
                      {...field}
                      min={new Date().toISOString().slice(0, 16)}
                    />
                  </FormControl>
                  <FormDescription>
                    When will the tournament begin?
                  </FormDescription>
                  <FormMessage />
                </FormItem>
              )}
            />

            <FormField
              control={form.control}
              name="maxParticipants"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Maximum Participants</FormLabel>
                  <FormControl>
                    <Input
                      type="number"
                      {...field}
                      onChange={(e) => field.onChange(parseInt(e.target.value))}
                    />
                  </FormControl>
                  <FormDescription>
                    Number of participants (2-128)
                  </FormDescription>
                  <FormMessage />
                </FormItem>
              )}
            />

            <FormField
              control={form.control}
              name="hasPrizePool"
              render={({ field }) => (
                <FormItem className="flex flex-row items-center justify-between rounded-lg border p-4">
                  <div className="space-y-0.5">
                    <FormLabel className="text-base">Prize Pool</FormLabel>
                    <FormDescription>
                      Enable if this tournament has prizes
                    </FormDescription>
                  </div>
                  <FormControl>
                    <Switch
                      checked={field.value}
                      onCheckedChange={field.onChange}
                    />
                  </FormControl>
                </FormItem>
              )}
            />

            {hasPrizePool && (
              <div className="space-y-4">
                <FormField
                  control={form.control}
                  name="prizePool"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Prize Amount</FormLabel>
                      <FormControl>
                        <Input
                          type="number"
                          {...field}
                          onChange={(e) =>
                            field.onChange(parseFloat(e.target.value))
                          }
                          min={0}
                          step="0.01"
                        />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />

                <FormField
                  control={form.control}
                  name="prizeCurrency"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Currency</FormLabel>
                      <Select
                        onValueChange={field.onChange}
                        defaultValue={field.value}
                      >
                        <FormControl>
                          <SelectTrigger>
                            <SelectValue placeholder="Select currency" />
                          </SelectTrigger>
                        </FormControl>
                        <SelectContent>
                          <SelectItem value="USD">USD</SelectItem>
                          <SelectItem value="EUR">EUR</SelectItem>
                          <SelectItem value="GBP">GBP</SelectItem>
                        </SelectContent>
                      </Select>
                      <FormMessage />
                    </FormItem>
                  )}
                />
              </div>
            )}

            <Button type="submit" className="w-full" disabled={isLoading}>
              {isLoading ? "Creating..." : "Create Tournament"}
            </Button>
          </form>
        </Form>
      </CardContent>
    </Card>
  );
}

export default TournamentForm;
